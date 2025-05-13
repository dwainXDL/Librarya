using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;

namespace Librarya.Classes
{
    internal class statsData
    {
        SqlConnection connection = new SqlConnection(session.connectionString);

        public DataTable getCategoryCounts()
        {
            string selectData = @"SELECT b.category, COUNT(*) AS issued_count FROM issues i JOIN books b ON i.bookID = b.bookID GROUP BY b.category;";
            using (SqlCommand cmd = new SqlCommand(selectData, connection))
            {
                DataTable dt = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                return dt;
            }
        }

        public DataTable getWeeklyCounts()
        {
            string selectData = @"SELECT CAST(i.issueDate AS DATE) AS issue_date, COUNT(*)               AS issued_count FROM issues i WHERE i.issueDate >= @startOfWeek AND i.issueDate <  @startOfNextWeek GROUP BY CAST(i.issueDate AS DATE) ORDER BY CAST(i.issueDate AS DATE);";

            DateTime today = DateTime.Today;
            int difference = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (difference < 0) difference += 7;
            DateTime startOfWeek = today.AddDays(-difference);
            DateTime startOfNextWeek = startOfWeek.AddDays(7);

            using (SqlCommand cmd = new SqlCommand(selectData, connection))
            {
                cmd.Parameters.AddWithValue("@startOfWeek", startOfWeek);
                cmd.Parameters.AddWithValue("@startOfNextWeek", startOfNextWeek);

                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
        }

        public void renderCategoryChart(DataTable dt, PlotView plotView1)
        {
            PlotModel model = new PlotModel 
            { 
                Title = "Issues By Category",
                DefaultFont = "Inknut Antiqua",
                DefaultFontSize = 13,
            };

            // 2) CategoryAxis on X (bottom)
            var categoryAxis = new CategoryAxis
            {
                Key = "CategoryAxis",
                Position = AxisPosition.Bottom,
                Title = "Category",
                Angle = 0,      // rotate labels if they overlap
                GapWidth = 0.5,
                Font = "Inknut Antiqua",
                FontSize = 13,
                FontWeight = FontWeights.Bold
            };
            foreach (DataRow row in dt.Rows)
                categoryAxis.Labels.Add(row["category"].ToString());
            model.Axes.Add(categoryAxis);

            // 3) Numeric axis on Y (left), stepping by 5
            var valueAxis = new LinearAxis
            {
                Key = "ValueAxis",
                Position = AxisPosition.Left,
                Title = "Issued Count",
                Minimum = 0,
                MajorStep = 5,
                MinorStep = 1,
                Font = "Inknut Antiqua",
                FontSize = 13,
                FontWeight = FontWeights.Bold
            };
            model.Axes.Add(valueAxis);

            // 4) BarSeries (horizontal) but mapped so categories are along X
            var series = new BarSeries
            {
                YAxisKey = "CategoryAxis",
                XAxisKey = "ValueAxis",
                LabelPlacement = LabelPlacement.Inside,
                BarWidth = 0.5,
                LabelFormatString = "{0}",
                // full‐alpha maroonish color
                FillColor = OxyColor.FromArgb(255, 92, 78, 78),
            };
            foreach (DataRow row in dt.Rows)
            {
                double count = Convert.ToDouble(row["issued_count"]);
                series.Items.Add(new BarItem { Value = count });
            }
            model.Series.Add(series);

            // 5) Render
            plotView1.Model = model;
        }

        public void renderEmptyChart(PlotView plotView)
        {
            // 1) New, empty PlotModel
            var model = new PlotModel
            {
                Title = "",
                DefaultFont = "Inknut Antiqua",
                DefaultFontSize = 13,
            };

            model.Axes.Add(new CategoryAxis { 
                Position = AxisPosition.Bottom, 
                Title = "X Axis",
                Minimum = 0,
                MajorStep = 101,
                MinorStep = 101,
                FontWeight = FontWeights.Bold,
            });
            model.Axes.Add(new LinearAxis { 
                Position = AxisPosition.Left, 
                Title = "Y Axis",
                Minimum = 0,
                MajorStep = 101,
                MinorStep = 101,
                FontWeight = FontWeights.Bold,
            });

            plotView.Model = model;
        }

        public void renderWeeklyChart(DataTable dt, PlotView plotView)
        {
            var model = new PlotModel
            {
                Title = "Books Issued This Week",
                DefaultFont = "Inknut Antiqua",
                DefaultFontSize = 13,
            };

            CategoryAxis dateAxis = new CategoryAxis
            {
                Key = "dateAxis",
                Position = AxisPosition.Bottom,
                Title = "Day",
                Angle = 0,
                GapWidth = 0.5,
                Font = "Inknut Antiqua",
                FontSize = 13,
                FontWeight = FontWeights.Bold
            };
            foreach (DataRow row in dt.Rows)
                dateAxis.Labels.Add(((DateTime)row["issue_date"]).ToString("dddd"));
            model.Axes.Add(dateAxis);

            LinearAxis countAxis = new LinearAxis
            {
                Key = "countAxis",
                Position = AxisPosition.Left,
                Title = "Issued Count",
                Minimum = 0,
                MajorStep = 1,
                MinorStep = 1,
                Font = "Inknut Antiqua",
                FontSize = 13,
                FontWeight = FontWeights.Bold
            };
            model.Axes.Add(countAxis);

            BarSeries series = new BarSeries
            {
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}",
                FillColor = OxyColor.FromArgb(255, 92, 78, 78),
                BarWidth = 0.5,
                XAxisKey = "countAxis",
                YAxisKey = "dateAxis",
            };

            foreach (DataRow row in dt.Rows)
                series.Items.Add(new BarItem { Value = Convert.ToDouble(row["issued_count"]) });
            model.Series.Add(series);

            plotView.Model = model;
        }
    }
}
