using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : Block
{
    protected override void Awake() {
        base.Awake();
        BlockType = BlockType.DIRT;
    }
}
