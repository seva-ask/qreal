using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QReal.Web.Database;

namespace QReal.Web
{
    public partial class AddInstances : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            using (var entities = new QRealEntities())
            {
                for (int i = 0; i < 10; i++)
                {
                    LogicalInstance logicalInstance = new LogicalInstance();
                    logicalInstance.Name = "Diagram" + i;
                    logicalInstance.Type = "ClassDiagram.ClassDiagramType";
                    entities.AddToLogicalInstances(logicalInstance);
                    RootInstance rootInstance = new RootInstance();
                    rootInstance.LogicalInstance = logicalInstance;
                    entities.AddToGraphicInstances(rootInstance);
                    for (int j = 0; j < 100; j++)
                    {
                        LogicalInstance logicalNodeInstance = new LogicalInstance();
                        logicalNodeInstance.Name = "Class" + j;
                        logicalNodeInstance.Type = "ClassDiagram.ClassType";
                        entities.AddToLogicalInstances(logicalNodeInstance);
                        NodeInstance nodeInstance = new NodeInstance();
                        nodeInstance.LogicalInstance = logicalNodeInstance;
                        nodeInstance.Parent = rootInstance;
                        nodeInstance.Height = 200 + random.Next(200);
                        nodeInstance.Width = 200 + random.Next(200);
                        nodeInstance.X = random.Next(600);
                        nodeInstance.Y = random.Next(600);
                        entities.AddToGraphicInstances(nodeInstance);
                    }
                }
                entities.SaveChanges();
            }
        }
    }
}