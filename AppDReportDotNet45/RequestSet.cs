using System;
using System.Collections.Generic;
using System.Text;

namespace AppdReporter
{
    class RequestSet
    {

        public ControllerConnection controllerConnection;
        DateParser dateParser;

        public RequestSet(int daysCount, String controllerUrl, String clientId, String clientAccount, String clientSecret)
        {
            controllerConnection = new ControllerConnection(controllerUrl, clientId, clientAccount,clientSecret);
            dateParser = new DateParser(daysCount);
        }

        public RequestSet(DateTime start, DateTime end, String controllerUrl , String clientId, String clientAccount, String clientSecret)
        {
            controllerConnection = new ControllerConnection(controllerUrl, clientId, clientAccount, clientSecret);
            dateParser = new DateParser(start,end);
        }


        public string prepareRequestsTimeSet()
        {
            return "start-time=" + dateParser.TimeTable[0] + "&end-time=" + dateParser.TimeTable[1];
        }



    }
}
