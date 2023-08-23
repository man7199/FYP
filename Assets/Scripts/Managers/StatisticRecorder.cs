// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Fusion;
// using UnityEngine;
//
// public class StatisticRecorder : NetworkBehaviour
// {
//     public static StatisticRecorder Instance;
//
//     // [Networked]  NetworkDictionary<PlayerRef, NetworkDictionary<int, int>> killStatistic { get; } 
//     [Networked]
//     [Capacity(10)]
//     [UnitySerializeField]
//     private NetworkDictionary<Type, int> Player1KillStatistic => default;
//
//     [Networked]
//     [Capacity(10)]
//     [UnitySerializeField]
//     private NetworkDictionary<Type, int> Player2KillStatistic => default;
//
//     [Networked] private PlayerRef player1Ref { get; set; }
//     [Networked] private PlayerRef player2Ref { get; set; }
//
//     private Type[] allUnitTypes =
//     {
//         typeof(Tank),
//         typeof(KillerCell),
//         typeof(NKCell),
//         typeof(Bacteria),
//         typeof(Corona)
//     };
//
//     private void Awake()
//     {
//         if (Instance != null)
//         {
//             Debug.LogError("There should be only one StatisticRecorder.");
//             return;
//         }
//
//         Instance = this;
//     }
//     
//     public bool AddPlayer(PlayerRef playerRef)
//     {
//         if (player1Ref == default)
//         {
//             player1Ref = playerRef;
//             foreach (Type unitType in allUnitTypes)
//             {
//                 Player1KillStatistic.Add(unitType, 0);
//                 Player2KillStatistic.Add(unitType, 0);
//             }
//         }
//         else
//         {
//             player2Ref = playerRef;
//             foreach (Type unitType in allUnitTypes)
//             {
//                 Player1KillStatistic.Add(unitType, 0);
//                 Player2KillStatistic.Add(unitType, 0);
//             }
//         }
//         return true;
//     }
//
//     public bool AddKillRecord(PlayerRef playerRef, Type unitType, int num = 1)
//     {
//         NetworkDictionary<Type, int> statistic;
//         if (playerRef == player1Ref)
//         {
//             statistic = Player1KillStatistic;
//         }
//         else
//         {
//             statistic = Player2KillStatistic;
//         }
//
//         int count = statistic.Get(unitType);
//         statistic.Set(unitType, count + num);
//         Debug.Log("Player " + playerRef + " has killed " + statistic.Get(unitType) + " "+unitType.ToString());
//         return true;
//     }
//
//     public int GetKillRecord(PlayerRef playerRef, Type unitType)
//     {
//         return Player1KillStatistic.Get(unitType);
//     }
//
//     // public override void FixedUpdateNetwork()
//     // {
//     //     Debug.Log("The kill number  is " + Player1KillStatistic.Get(Unit.UnitType.KillerCell));
//     // }
//
//     // public bool AddPlayer(PlayerRef playerRef)
//     // {
//     //     NetworkDictionary<int,int> dictionary = new NetworkDictionary<int, int>();
//     //     IEnumerable<Unit.UnitType> unitTypes = Enum.GetValues(typeof(Unit.UnitType)).Cast<Unit.UnitType>();
//     //     foreach (Unit.UnitType unitType in unitTypes)
//     //     {
//     //         dictionary.Add((int) unitType,0);
//     //     }
//     //     return killStatistic.Add(playerRef,dictionary);
//     // }
//     //
//     // public bool AddKillRecord(PlayerRef playerRef, Unit.UnitType unitType, int num=1)
//     // {
//     //     NetworkDictionary<int,int> dictionary = killStatistic[playerRef];
//     //     dictionary[(int) unitType] += num;
//     //     return true;
//     // }
//     //
//     // public int GetKillRecord(PlayerRef playerRef, Unit.UnitType unitType)
//     // {
//     //     NetworkDictionary<int,int> dictionary = killStatistic[playerRef];
//     //     return dictionary[(int)unitType];
//     // }
//     //
//     // public override void FixedUpdateNetwork()
//     // {
//     //     foreach (var (playerRef, value) in killStatistic)
//     //     {
//     //         int i = value[(int) Unit.UnitType.KillerCell];
//     //         Debug.Log("The kill number of "+playerRef+" is "+i);
//     //     }
//     // }
// }