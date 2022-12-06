using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTDecorator : BTBaseNode
{
    protected BTBaseNode child;

    public BTDecorator(BlackBoard _blackBoard, BTBaseNode _node) : base(_blackBoard)
    {
        child = _node;
    }
}
