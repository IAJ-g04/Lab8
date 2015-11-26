using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameManager
{
    public class GameManager : MonoBehaviour
    {
        private const float UPDATE_INTERVAL = 0.5f;
        //public fields, seen by Unity in Editor
        public GameObject character;

        public Text HPText;
        public Text EnergyText;
        public Text HungerText;
        public Text ArrowsText;
        public Text MoneyText;

        //private fields
        public List<GameObject> chests;
        public List<GameObject> trees;
        public List<GameObject> boars;
        public List<GameObject> beds;
        public Dictionary<Vector3, GameObject> redFlags;
        public Dictionary<Vector3, GameObject> greenFlags; 

        public CharacterData characterData; 
 
        private float nextUpdateTime = 0.0f;
        private Vector3 previousPosition;

        public void Start()
        {
            this.characterData = new CharacterData(this.character);
            this.previousPosition = this.character.transform.position;

            this.chests = GameObject.FindGameObjectsWithTag("Chest").ToList();
            this.trees = GameObject.FindGameObjectsWithTag("Tree").ToList();
            this.beds = GameObject.FindGameObjectsWithTag("Bed").ToList();
            this.boars = GameObject.FindGameObjectsWithTag("Boar").ToList();
            this.redFlags = new Dictionary<Vector3, GameObject>();
        }

        public void Update()
        {
            if (Time.time > this.nextUpdateTime)
            {
                this.nextUpdateTime = Time.time + UPDATE_INTERVAL;

                //lower energy depending on the distance traversed since the last update
                var distance = (this.character.transform.position - this.previousPosition).magnitude;
                this.previousPosition = this.character.transform.position;
                var velocity = distance/UPDATE_INTERVAL;
                if (velocity > 20)
                {
                    this.characterData.Energy -= 0.02f*distance;
                }
                else if (velocity > 0)
                {
                    this.characterData.Energy -= 0.01f*distance;
                }

                //increase hunger over time (0.1f per second)
                this.characterData.Hunger += 0.05f;
                if (this.characterData.Hunger > 10.0f)
                {
                    this.characterData.Hunger = 10.0f;
                }
            }

            this.HPText.text = "HP: " + this.characterData.HP;
            this.EnergyText.text = "Energy: " + this.characterData.Energy;
            this.HungerText.text = "Hunger: " + this.characterData.Hunger;
            this.ArrowsText.text = "Arrows: " + this.characterData.Arrows;
            this.MoneyText.text = "Money: " + this.characterData.Money;
        }


        public void MeleeAttack(GameObject boar)
        {
            if (boar.activeSelf && InBoarMeleeRange(boar))
            {
                this.boars.Remove(boar);
                GameObject.DestroyObject(boar);
                this.characterData.HP -= 2;
                this.characterData.Hunger -= 2;
                this.characterData.Energy -= 0.5f;
            }
        }

        public void Shoot(GameObject boar)
        {
            if (boar.activeSelf && InShootRange(boar) && this.characterData.Arrows > 0)
            {
                this.boars.Remove(boar);
                GameObject.DestroyObject(boar);
                this.characterData.Hunger -= 2;
                this.characterData.Arrows --;
            }
        }

        public void PickUpChest(GameObject chest)
        {
            if (chest.activeSelf && InChestRange(chest))
            {
                this.boars.Remove(chest);
                GameObject.DestroyObject(chest);
                this.characterData.Money += 5;
            }
        }

        public void GetArrows(GameObject tree)
        {
            if (InTreeRange(tree))
            {
                this.characterData.Arrows = 10;
            }
        }

        public void Rest()
        {
            //you can rest anywhere and at anytime, but it doesn't give much energy
            this.characterData.Energy += 0.1f;
            if (this.characterData.Energy > 10.0f)
            {
                this.characterData.Energy = 10.0f;
            }
        }

        public void Sleep(GameObject bed)
        {
            if (InBedRange(bed))
            {
                this.characterData.Energy += 1.0f;
                if (this.characterData.Energy > 10.0f)
                {
                    this.characterData.Energy = 10.0f;
                }
            }
        }


        private bool CheckRange(GameObject obj, float maximumSqrDistance)
        {
            return (obj.transform.position - this.characterData.CharacterGameObject.transform.position).sqrMagnitude <= maximumSqrDistance;
        }


        public bool InBoarMeleeRange(GameObject boar)
        {
            return this.CheckRange(boar, 1.0f);
        }

        public bool InShootRange(GameObject boar)
        {
            return this.CheckRange(boar, 16.0f);
        }

        public bool InChestRange(GameObject chest)
        {
            return this.CheckRange(chest, 1.0f);
        }

        public bool InBedRange(GameObject bed)
        {
            return this.CheckRange(bed, 4.0f);
        }

        public bool InTreeRange(GameObject tree)
        {
            return this.CheckRange(tree, 4.0f);
        }



    }
}
