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

    private PlayerStats _playerStats;

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
            if (_playerStats == null)
            {
                _playerStats = target.GetComponent<PlayerStats>();
            }

            Vector3 _distance = target.position - transform.position;

            if (_distance.sqrMagnitude >= _stopDistance * _stopDistance)
            {
                Move(_distance.normalized, _moveSpeed);
            }
            else
            {
                enemyCombat.Attack(_playerStats);
            }

            //TODO: add line of sight check

        }
    }
}