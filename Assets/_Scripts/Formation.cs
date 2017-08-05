using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation {

    private LinkedList<EnemyPosition> enemypositions = new LinkedList<EnemyPosition>();
    
    public Formation(LinkedList<EnemyPosition> enemypositions)
    {
        this.enemypositions = enemypositions;
    }


}
