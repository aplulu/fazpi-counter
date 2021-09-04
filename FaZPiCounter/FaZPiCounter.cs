using System.Collections.Generic;
using CountersPlus.Counters.Custom;
using SiraUtil.Tools;
using TMPro;
using UnityEngine;
using Zenject;


namespace FaZPiCounter
{
    public class FaZPiCounter: BasicCustomCounter, ISaberSwingRatingCounterDidFinishReceiver
    {
        [Inject]
        private SiraLog _log = null;
        
        [Inject]
        private readonly BeatmapObjectManager _beatmapObjectManager = null;

        [Inject] private readonly PluginConfig _config = null;
        
        private readonly Dictionary<ISaberSwingRatingCounter, float> _noteCutMap = new Dictionary<ISaberSwingRatingCounter, float
        >();

        private int _belowThresholdCount = 0;
        private TMP_Text _counter;
        
        public override void CounterInit()
        {
            _beatmapObjectManager.noteWasCutEvent += OnNoteWasCut;

            var label = CanvasUtility.CreateTextFromSettings(Settings);
            label.text = "FaZPi Counter";
            
            _counter = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(0, -0.35f, 0));
            
            UpdateCounter();
        }

        public override void CounterDestroy()
        {
            _beatmapObjectManager.noteWasCutEvent -= OnNoteWasCut;
        }

        private void UpdateCounter()
        {
            _counter.text = $"{_belowThresholdCount}";
        }

        private void OnNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            if (noteCutInfo.swingRatingCounter == null) return;
            _noteCutMap[noteCutInfo.swingRatingCounter] = noteCutInfo.cutDistanceToCenter;
            noteCutInfo.swingRatingCounter.RegisterDidFinishReceiver(this);
        }

        public void HandleSaberSwingRatingCounterDidFinish(ISaberSwingRatingCounter saberSwingRatingCounter)
        {
            var cutDistanceToCenter = _noteCutMap[saberSwingRatingCounter];
            ScoreModel.RawScoreWithoutMultiplier(saberSwingRatingCounter, cutDistanceToCenter, out var beforeCutScore,
                out var afterCutScore, out var distanceScore);
            var score = beforeCutScore + afterCutScore + distanceScore;
            if (score < _config.belowThreshold)
            {
                _belowThresholdCount++;
                UpdateCounter();
            }
        }
    }
}