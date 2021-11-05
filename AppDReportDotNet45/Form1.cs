using AppdReporter.ReportCache;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AppdReporter
{
    public partial class Form1 : Form
    {

        


        public Form1()
        {
            InitializeComponent();
            Configuration.ReportStyle = "Timemachine";
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            //backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            

        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportDbExporter reportDbExporter = new ReportDbExporter("visual");
            reportDbExporter.reportToListView(listView1);
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            pictureBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PreviousRadio.Checked == true)
            {
                Configuration.ReportStyle = "Timemachine";
                Configuration.TimeMachineValue = Int32.Parse(textBox3.Text);
            } else
            {
                Configuration.ReportStyle = "Custom";
                Configuration.StartTime = startDatePicker.Value.Date + startTimePicker.Value.TimeOfDay;
                Configuration.EndTime = endDatePicker.Value.Date + endTimePicker.Value.TimeOfDay;
            }

            listView1.Items.Clear();

            
            backgroundWorker1.RunWorkerAsync();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            pictureBox1.Visible = true;

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Configuration.LoadConfiguration();
            controllerUrlTxtBox.Text = Configuration.ControllerUrl;
            ControllerAccountTxt.Text = Configuration.ControllerAccount;
            ControllerApiKeyTxt.Text = Configuration.ApiId;
            ControllerSecretKeyTxt.Text = Configuration.SecretKey;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DateParser dateParser1 = new DateParser();
            ReportInitializer reportInitializer = new ReportInitializer();
            RequestSet dateParser;
            if (Configuration.ReportStyle == "Timemachine")
            {
                dateParser = new RequestSet(Configuration.TimeMachineValue, Configuration.ControllerUrl, Configuration.ApiId, Configuration.ControllerAccount, Configuration.SecretKey);
            }
            else
            {
                dateParser = new RequestSet(Configuration.StartTime, Configuration.EndTime, Configuration.ControllerUrl, Configuration.ApiId, Configuration.ControllerAccount, Configuration.SecretKey);
            }

            JArray valut = dateParser.controllerConnection.getHealthRuleViolations("ERROR", dateParser.prepareRequestsTimeSet());
            for (int i = 0; i < valut.Count; i++)
            {
                reportInitializer.sqliteDbInsertRecord(valut[i].SelectToken("affectedEntityDefinition.name").ToString(),
                    valut[i].SelectToken("triggeredEntityDefinition.name").ToString(),
                    dateParser1.epochToReadable(valut[i].SelectToken("startTimeInMillis").ToString()),
                    dateParser1.durationInMinutes(valut[i].SelectToken("startTimeInMillis").ToString(), valut[i].SelectToken("endTimeInMillis").ToString()));
            }
            GC.Collect();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           
            SaveFileDialog savefile = new SaveFileDialog();
            // set a default file name
            savefile.FileName = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +"_Report.xls";
            // set filters - this can be done in properties as well
            savefile.Filter = "Excel |*.xls";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                ReportDbExporter reportDbExporter = new ReportDbExporter();
                reportDbExporter.prepareExcelReport(savefile.FileName);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var picker = new DateTimePicker();
            Form f = new Form();
            f.Controls.Add(picker);

            var result = f.ShowDialog();
            if (result == DialogResult.OK)
            {
                //get selected date
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int32.Parse(textBox3.Text);
            } catch (Exception)
            {
                textBox3.Text = "1";
            }
        }

        private void CustomRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (CustomRadio.Checked == true)
               PreviousRadio.Checked = false;
           
        }

        private void PreviousRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (PreviousRadio.Checked == true)
               CustomRadio.Checked = false;
           
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Configuration.ControllerUrl = controllerUrlTxtBox.Text;
            Configuration.ControllerAccount = ControllerAccountTxt.Text;
            Configuration.ApiId = ControllerApiKeyTxt.Text;
            Configuration.SecretKey = ControllerSecretKeyTxt.Text;
            Configuration.SaveConfiguration();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
