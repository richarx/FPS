using System.Collections;
using Player.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons.Throw
{
    public class AnimateThrow : MonoBehaviour
    {
        [SerializeField] private GameObject thumb;
        [SerializeField] private GameObject pivot;
        [SerializeField] private Image hand;
        
        [Space]
        [SerializeField] private Sprite regular;
        
        [Space]
        [SerializeField] private Sprite throw_1;
        [SerializeField] private float delay_1;
        
        [Space]
        [SerializeField] private Sprite throw_2;
        [SerializeField] private float delay_2;
        
        private PlayerStateMachine player;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            
            player.playerTools.OnThrowItem.AddListener(TriggerAnimation);
        }

        private void TriggerAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(TriggerAnimationCoroutine());
        }

        private IEnumerator TriggerAnimationCoroutine()
        {
            thumb.SetActive(false);
            pivot.SetActive(false);
            
            hand.sprite = throw_1;
            yield return new WaitForSeconds(delay_1);
            
            hand.sprite = throw_2;
            yield return new WaitForSeconds(delay_2);

            hand.sprite = regular;
            thumb.SetActive(true);
            pivot.SetActive(true);
        }
    }
}
