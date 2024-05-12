﻿using System;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(HealthBar))]
    public class PlayerHealthBar : MonoBehaviour
    {
        private HealthBar _healthBar;

        private void Awake()
        {
            _healthBar = GetComponent<HealthBar>();
        }

        private void Start()
        {
            var player = ServiceProvider.Instance.Get<IPlayer>();
            player.HealthChanged.AddListener(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged(int currHealth, int maxHealth)
        {
            _healthBar.OnHealthChanged(currHealth, maxHealth);
        }
    }
}