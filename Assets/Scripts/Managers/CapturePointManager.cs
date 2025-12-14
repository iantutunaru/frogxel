using System;
using System.Collections.Generic;
using System.Linq;
using Frogxel.Lanes;
using Player;
using UnityEngine;

namespace Managers
{
    public class CapturePointManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private GameManager gameManager;

        [Header("Score points")] 
        [SerializeField] private int pointsForCapturing = 500;
        [SerializeField] private int pointsForCapturingLastPoint = 1000;
        
        private List<CapturePoint> _capturePoints = new();
        
        public void ResetCapturePoints()
        {
            if (_capturePoints.Count <= 0)
            {
                throw new Exception("No capture points found");
            }
            
            foreach (var capturePoint in _capturePoints)
            {
                capturePoint.Clear();
            }
        }
        
        private void OnEnable()
        {
            CapturePointsLaneController.CapturePointsInitialised += OnCapturePointsInitialised;
            CapturePoint.Captured += OnPointCaptured;
        }

        private void OnDisable()
        {
            CapturePointsLaneController.CapturePointsInitialised -= OnCapturePointsInitialised;
            CapturePoint.Captured -= OnPointCaptured;
        }

        private void RespawnPlayer()
        {
            gameManager.Respawn();
        }

        private void AllBasesCaptured()
        {
            gameManager.AllBasesCaptured();
        }
        
        /// <summary>
        /// Go though each home and check if it's occupied
        /// </summary>
        private bool Cleared()
        {
            // Go through each home
            return _capturePoints.All(capturePoint => capturePoint.IsCaptured);
        }
        
        private void OnCapturePointsInitialised(List<CapturePoint> capturePoints)
        {
            _capturePoints = new List<CapturePoint>(capturePoints);
        }
        
        /// <summary>
        /// When home is captured by the player then disable controls of that player, increase their score by a set amount and check if all homes have been captured.
        /// If all homes have been captured and this was the last home then give player a double reward and pause the game. Otherwise, respawn the player.
        /// </summary>
        private void OnPointCaptured(PlayerController player)
        {
            player.SetWon(true);

            scoreManager.SetScore(player, player.GetPlayerStats().GetScore() + pointsForCapturing);

            // Check if all homes have been captured
            if (Cleared())
            {
                scoreManager.SetScore(player, player.GetPlayerStats().GetScore() + pointsForCapturingLastPoint);
                StopAllCoroutines();
                Invoke(nameof(AllBasesCaptured), 1f);
            }
            else
            {
                Invoke(nameof(RespawnPlayer), 1f);
            }
        }
    }
}
