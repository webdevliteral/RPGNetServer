using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class EnemyController : AIController
{
    [SerializeField]
    private Combat enemyCombat;

    protected float _stopDistance = 2.0f;
    protected float _moveSpeed = 3.5f;

    protected override void Awake()
    {
        base.Awake();
        enemyCombat = GetComponent<Combat>();
        
    }
    void FixedUpdate()
    {
        if (target == null)
            SearchForPlayersInsideRadius();

        if (target != null)
        {
            Vector3 _distance = target.position - transform.position;
            PlayerStats _playerStats = target.GetComponent<PlayerStats>();
            if (_distance.sqrMagnitude >= _stopDistance * _stopDistance)
            {
                Move(_distance.normalized, _moveSpeed);
            }
            else
            {
                enemyCombat.Attack(_playerStats);
            }
            
            //if (TargetInLineOfSight(target))

        }
    }

    private bool TargetInLineOfSight(Transform _target)
    {

        if (Physics.Raycast(transform.position, _target.transform.position, out RaycastHit _hit, _lookRadius))
        {
            if (_hit.collider != null)
            {
                return true;
            }
        }

        return false;
    }
}