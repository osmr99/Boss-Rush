#pragma warning disable IDE0051
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Rendering;

namespace Omar
{
    public class OmarPlayerHealthBarManager : MonoBehaviour
    {
        //[SerializeField] PlayerHealthDisplay healthDisplay;
        [SerializeField] Color heartColor;
        [SerializeField] int numOfHearts;

        List<GameObject> hearts;
        [SerializeField] Sprite heart;
        [SerializeField] Sprite empty;
        Transform _transform;
        private void Awake()
        {
            hearts = new List<GameObject>();
            _transform = transform;
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SetHealthBar());
        }

        IEnumerator SetHealthBar()
        {
            yield return new WaitForSeconds(0.1f);
            SetMaxHearts(numOfHearts);
            UpdateHearts(0, numOfHearts);
            foreach (Image heart in this.GetComponentsInChildren<Image>())
            {
                heart.color = heartColor;
            }
        }
        
        public void SetMaxHearts(int maxHealth)
        {
            for (int i = 0; i < hearts.Count; i++)
                Destroy(hearts[i]);

            hearts.Clear();

            for (int i = 0; i < maxHealth; i++)
            {
                var current = new GameObject();
                var image = current.AddComponent<Image>();
                image.sprite = empty;

                current.transform.SetParent(_transform);
                current.GetComponent<Image>().color = heartColor;

                hearts.Add(current);
            }
        }

        public void UpdateHearts(int damageAmount, int newCurrent)
        {
            // reset to all being empty and white
            SetMaxHearts(hearts.Count);

            // ensure that only as many as we currently have are filled
            for (int i = 0; i < newCurrent; i++)
            {
                var currentHeart = hearts[i].GetComponent<Image>();
                currentHeart.sprite = heart;
                currentHeart.color = heartColor;
            }
        }
    }
}
