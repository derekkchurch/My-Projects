using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SchemaMaster
{
    public partial class Results : Form
    {
        public Results(DataSet _dsResults, string title)
        {
            InitializeComponent();

            this.gridDropped.DataSource = _dsResults.Tables[0];
            this.gridDropped.Columns[0].Width = 200;

            this.gridNew.DataSource = _dsResults.Tables[1];
            this.gridNew.Columns[0].Width = 200;

            this.gridChanged.DataSource = _dsResults.Tables[2];

            this.gridDroppedColumns.DataSource = _dsResults.Tables[3];
            this.gridDroppedColumns.Columns[0].Width = 300;
            this.gridDroppedColumns.Columns[1].Width = 100;
            this.gridDroppedColumns.Columns[2].Width = 100;

            this.gridNewColumns.DataSource = _dsResults.Tables[4];

            this.labelTitle.Text = title;
        }
    }
}
