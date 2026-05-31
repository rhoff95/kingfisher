using System;
using Scripts;
using UnityEngine;

namespace Actors.Enemies
{
    [RequireComponent(typeof(Actor))]
    public abstract class Enemy : MonoBehaviour
    {
        protected Actor Actor;
        protected Transform ActorTransform;
        protected GameManager _gameManager;

        [TagSelector] public string playerTag;

        protected void Awake()
        {
            Actor = GetComponent<Actor>();

            var player = GameObject.FindGameObjectWithTag(playerTag);
            ActorTransform = player.transform;

            _gameManager = GameManager.Instance;
        }

        protected void Start()
        {
            Actor.onDeathCallback += () => _gameManager.RemoveEnemy(this);
        }
    }
}