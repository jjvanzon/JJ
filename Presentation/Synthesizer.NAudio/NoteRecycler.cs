﻿using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JJ.Business.Synthesizer.Helpers;
using JJ.Framework.Exceptions;

namespace JJ.Presentation.Synthesizer.NAudio
{
    public class NoteRecycler
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private IList<NoteInfo> _noteInfos;

        public NoteRecycler(int maxConcurrentNotes)
        {
            SetMaxConcurrentNotes(maxConcurrentNotes);
        }

        public void SetMaxConcurrentNotes(int maxConcurrentNotes)
        {
            if (maxConcurrentNotes < 1) throw new LessThanException(() => maxConcurrentNotes, 1);

            _lock.EnterWriteLock();
            try
            {
                _noteInfos = new NoteInfo[maxConcurrentNotes];

                for (int i = 0; i < maxConcurrentNotes; i++)
                {
                    // TODO: Clone info if needed or do full-CRUD.
                    _noteInfos[i] = new NoteInfo
                    {
                        ListIndex = i,
                        EndTime = CalculationHelper.VERY_LOW_VALUE,
                        ReleaseTime = CalculationHelper.VERY_LOW_VALUE,
                        StartTime = CalculationHelper.VERY_LOW_VALUE
                    };
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary> Returns null if max concurrent notes was exceeded. </summary>
        public NoteInfo TryGetNoteInfoToStart(int noteNumber, double presentTime)
        {
            NoteInfo noteInfo;

            _lock.EnterReadLock();
            try
            {
                noteInfo = _noteInfos.Where(x => x.EndTime < presentTime).FirstOrDefault();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            // ReSharper disable once InvertIf
            if (noteInfo != null)
            {
                noteInfo.NoteNumber = noteNumber;
                noteInfo.StartTime = presentTime;
                noteInfo.ReleaseTime = CalculationHelper.VERY_HIGH_VALUE;
                noteInfo.EndTime = CalculationHelper.VERY_HIGH_VALUE;
            }

            return noteInfo;
        }

        /// <summary> Might return null, when note was ignored earlier, due to not enough slots. </summary>
        public NoteInfo TryGetNoteInfoToRelease(int noteNumber, double presentTime)
        {
            _lock.EnterReadLock();
            try
            {
                NoteInfo noteInfo = _noteInfos.Where(
                                                  x => x.NoteNumber == noteNumber &&
                                                       x.ReleaseTime > presentTime &&
                                                       x.EndTime > presentTime) // Should never be evaluated, but does not cost anything to keep it in there.
                                              .OrderBy(x => x.StartTime)
                                              .FirstOrDefault();
                return noteInfo;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void ReleaseNoteInfo(NoteInfo noteInfo, double releaseTime, double endTime)
        {
            if (noteInfo == null) throw new NullException(() => noteInfo);

            noteInfo.ReleaseTime = releaseTime;
            noteInfo.EndTime = endTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool NoteIsReleased(int noteListIndex, double presentTime)
        {
            if (noteListIndex < 0) throw new LessThanException(() => noteListIndex, 0);
            if (noteListIndex > _noteInfos.Count) throw new GreaterThanException(() => noteListIndex, () => _noteInfos.Count);

            _lock.EnterReadLock();
            try
            {
                bool isReleased = _noteInfos[noteListIndex].EndTime < presentTime;
                return isReleased;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}