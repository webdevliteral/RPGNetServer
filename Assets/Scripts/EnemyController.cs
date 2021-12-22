using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(NetworkComponent))]
class EnemyController : AIController
{
    [SerializeField]
    Combat enemyCombat;

    private NetworkComponent networkComponent;

    private void Awake()
    {
        enemyCombat = GetComponent<Combat>();
        networkComponent = GetComponent<NetworkComponent>();
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
                ServerSend.UpdateEnemyPosition(networkComponent.NetworkId, transform.position);
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