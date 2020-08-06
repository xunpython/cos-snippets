using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Model.Bucket;
using COSXML.Model.Service;
using COSXML.Utils;
using COSXML.Auth;
using COSXML.Transfer;
using System;
using COSXML;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COSSnippet
{
    public class BucketInventoryModel {

      private CosXml cosXml;

      BucketInventoryModel() {
        CosXmlConfig config = new CosXmlConfig.Builder()
          .SetConnectionTimeoutMs(60000)  //设置连接超时时间，单位毫秒，默认45000ms
          .SetReadWriteTimeoutMs(40000)  //设置读写超时时间，单位毫秒，默认45000ms
          .IsHttps(true)  //设置默认 HTTPS 请求
          .SetAppid("1250000000") //设置腾讯云账户的账户标识 APPID
          .SetRegion("COS_REGION") //设置一个默认的存储桶地域
          .Build();
        
        string secretId = "COS_SECRETID";   //云 API 密钥 SecretId
        string secretKey = "COS_SECRETKEY"; //云 API 密钥 SecretKey
        long durationSecond = 600;          //每次请求签名有效时长，单位为秒
        QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, 
          secretKey, durationSecond);
        
        this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
      }

      /// 设置存储桶清单任务
      public void PutBucketInventory()
      {
        //.cssg-snippet-body-start:[put-bucket-inventory]
        try
        {
          string inventoryId = "aInventoryId";
          string bucket = "examplebucket-1250000000"; //格式：BucketName-APPID
          PutBucketInventoryRequest putRequest = new PutBucketInventoryRequest(bucket, inventoryId);
          putRequest.SetDestination("CSV", "100000000001", "examplebucket-1250000000", "ap-guangzhou","list1");
          putRequest.IsEnable(true);
          putRequest.SetScheduleFrequency("Daily");
          //执行请求
          PutBucketInventoryResult putResult = cosXml.putBucketInventory(putRequest); 
          
          //请求成功
          Console.WriteLine(putResult.GetResultInfo());
        }
        catch (COSXML.CosException.CosClientException clientEx)
        {
          //请求失败
          Console.WriteLine("CosClientException: " + clientEx);
        }
        catch (COSXML.CosException.CosServerException serverEx)
        {
          //请求失败
          Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        }
        //.cssg-snippet-body-end
      }

      /// 获取存储桶清单任务
      public void GetBucketInventory()
      {
        //.cssg-snippet-body-start:[get-bucket-inventory]
        try
        {
          string inventoryId = "aInventoryId";
          string bucket = "examplebucket-1250000000"; //格式：BucketName-APPID
          GetBucketInventoryRequest getRequest = new GetBucketInventoryRequest(bucket);
          getRequest.SetInventoryId(inventoryId);
          
          GetBucketInventoryResult getResult = cosXml.getBucketInventory(getRequest);
          
          InventoryConfiguration configuration = getResult.inventoryConfiguration;
        }
        catch (COSXML.CosException.CosClientException clientEx)
        {
          //请求失败
          Console.WriteLine("CosClientException: " + clientEx);
        }
        catch (COSXML.CosException.CosServerException serverEx)
        {
          //请求失败
          Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        }
        //.cssg-snippet-body-end
      }

      /// 删除存储桶清单任务
      public void DeleteBucketInventory()
      {
        //.cssg-snippet-body-start:[delete-bucket-inventory]
        try
        {
          string inventoryId = "aInventoryId";
          string bucket = "examplebucket-1250000000"; //格式：BucketName-APPID
          DeleteBucketInventoryRequest deleteRequest = new DeleteBucketInventoryRequest(bucket);
          deleteRequest.SetInventoryId(inventoryId);
          DeleteBucketInventoryResult deleteResult = cosXml.deleteBucketInventory(deleteRequest);
          
          //请求成功
          Console.WriteLine(deleteResult.GetResultInfo());
        }
        catch (COSXML.CosException.CosClientException clientEx)
        {
          //请求失败
          Console.WriteLine("CosClientException: " + clientEx);
        }
        catch (COSXML.CosException.CosServerException serverEx)
        {
          //请求失败
          Console.WriteLine("CosServerException: " + serverEx.GetInfo());
        }
        //.cssg-snippet-body-end
      }

      // .cssg-methods-pragma

      static void Main(string[] args)
      {
        BucketInventoryModel m = new BucketInventoryModel();

        /// 设置存储桶清单任务
        m.PutBucketInventory();
        /// 获取存储桶清单任务
        m.GetBucketInventory();
        /// 删除存储桶清单任务
        m.DeleteBucketInventory();
        // .cssg-methods-pragma
      }
    }
}