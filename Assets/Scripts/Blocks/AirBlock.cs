using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBlock : Block
{
    protected override void Awake() {
        base.Awake();
        BlockType = BlockType.AIR;
    }
}
