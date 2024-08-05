using System.ServiceModel;

namespace HealthCare.Services
{



    [ServiceContract]
    public interface ISoapService
    {

        [OperationContract]
        string Sum(int num1, int num2);


    }
    public class SoapService : ISoapService
    {

         public string Sum(int num1, int num2)
        {
            return $"the sum of two number is  : {num1 + num2}";
        }
    }
}
