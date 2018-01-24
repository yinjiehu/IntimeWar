using UnityEngine;
using System;

namespace YJH.Unit
{
    public interface IUnitMobilityInput
    {
        bool Enabled { get; }
        Vector3? NormalizedMoveDirection { get; }
    }

    public interface IUnitAttachmentInput
    {
        bool Enabled { get; }

        bool[] AttachmentPress { get; }
        bool[] AttachmentHolding { get; }
        bool[] AttachmentRelease { get; }
        bool[] AttachmentClicked { get; }

        bool MainFireControlPress { get; }
        bool MainFireControlHoding { get; }
        bool MainFireControlRelease { get; }
    }
}