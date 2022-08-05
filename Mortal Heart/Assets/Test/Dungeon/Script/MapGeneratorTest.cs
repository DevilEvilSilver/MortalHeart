using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MapGeneratorTest : MonoBehaviour
{
    public GameObject normal;
    public GameObject elite;
    public GameObject shop;
    public GameObject boss;

    public int width, height;

    [Button("SpawnMap")]
    public void SpawnMap()
    {
        DungeonController.Instance.InitDungeon();
        var map = DungeonController.Instance.Map;

        SimplePool.Spawn(normal, new Vector3((width / 2 - width / 2f) * 2.5f, (-1 - height / 2f) * 2.5f)
            , Quaternion.identity);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i, j].isActive)
                {
                    Vector3 pos = new Vector3((i - width / 2f) * 2.5f, (j - height / 2f) * 2.5f, 0f);
                    switch (map[i ,j].type)
                    {
                        case RoomType.Normal:
                            SimplePool.Spawn(normal, pos, Quaternion.identity);
                            break;
                        case RoomType.Elite:
                            SimplePool.Spawn(elite, pos, Quaternion.identity);
                            break;
                        case RoomType.Shop:
                            SimplePool.Spawn(shop, pos, Quaternion.identity);
                            break;
                        case RoomType.Boss:
                            SimplePool.Spawn(boss, pos, Quaternion.identity);
                            break;
                    }
                }
                
            }
        }
    }
}
