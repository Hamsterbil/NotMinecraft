using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBlock : Block
{
    protected override void Awake() {
        base.Awake();
        BlockType = BlockType.GOLD;
    }
}
