using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

public class StartProgram : MonoBehaviour
{
    CognitoAWSCredentials credentials;

    DynamoDBContext context;
    AmazonDynamoDBClient client;

    void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        credentials = new CognitoAWSCredentials("ap-northeast-2:c9cc8d89-480e-494c-b6c9-17536ba48298", RegionEndpoint.APNortheast2);

        client = new AmazonDynamoDBClient(credentials, RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(client);


        CreateTest();
        FindItem();
    }

    [DynamoDBTable("Item")]
    public class Item
    {
        [DynamoDBHashKey]
        public string id { get; set; }

        [DynamoDBProperty]
        public string name { get; set; }

        [DynamoDBProperty]
        public string description { get; set; }

        [DynamoDBProperty]
        public int value { get; set; }
    }

    private void CreateTest()
    {
        Item i1 = new Item { id = "food_3", name = "guacamoly", description = "나쵸와 아보카도가 적절하게 섞인 음식", value = 1000 };
        context.SaveAsync(i1, (result) =>
        {
            if (result.Exception == null)
                Debug.Log("Success!");
            else
                Debug.Log(result.Exception);
        });
    }


    public void FindItem()
    {
        Item i;
        context.LoadAsync<Item>("food_2", (AmazonDynamoDBResult<Item> result) =>
        {
            if (result.Exception != null)
            {
                Debug.LogException(result.Exception);
                return;
            }

            i = result.Result;
            Debug.Log($"id : {i.id}");
            Debug.Log($"name : {i.name}");
            Debug.Log($"value : {i.value}");
            Debug.Log($"description : {i.description}");
        }, null);
    }
}
