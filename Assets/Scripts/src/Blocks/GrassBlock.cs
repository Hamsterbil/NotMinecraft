using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlock : Block
{
    protected override void Awake() {
        base.Awake();
        BlockType = BlockType.GRASS;
    }
}
