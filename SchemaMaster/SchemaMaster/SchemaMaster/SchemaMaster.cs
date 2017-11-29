using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SchemaMaster
{
    public partial class SchemaMaster : Form
    {
        
        private DataSet _dsMain, _dsResults;
        private DataTable _dtSet, _dtDBName, _dtCurOld, _dtCurNew;
        private DataView _dvOldRelease, _dvNewRelease;
        private const string _connString = @"Initial Catalog=TylerDBA;Data Source=PLADPSVDEVDB1\PHOENIX;Integrated Security=SSPI;";     //"Server=localhost;Database=DataDictionary;User Id=DataDictionary;Password=tyler-tech;";
        private SqlConnection _conn;
        private SqlDataAdapter _dataAdapter;
        private string _resultsTitle;
        private string _currentOld, _currentNew;
 
        public SchemaMaster()
        {
            InitializeComponent();
            _dsMain = new DataSet("Main");
            _conn = new SqlConnection(_connString);
            _conn.Open();
            _dtSet = new DataTable();
            //_dtOld = new DataTable();
        }

        private void GetSetData()
        {
//            _dtSet = new DataTable();
//            _dataAdapter = new SqlDataAdapter("SELECT SetID, SetKey, DBName, Release, Patch, CAST(Release AS VARCHAR) + '.' + CAST(Patch AS VARCHAR) AS RelPatch, Revision FROM DataDictionary.[Set] ORDER BY DBName, SetKey", _conn);
            _dataAdapter = new SqlDataAdapter("SELECT SetID, SetKey, DBName, Release, Patch, CAST(Release AS VARCHAR) + '.' + CAST(Patch AS VARCHAR) AS RelPatch, Revision FROM DataDictionary.[Set] ORDER BY DBName, Release, Patch, Revision", _conn);
            //_dataAdapter.Fill(_dsMain, "Set");
            _dataAdapter.Fill(_dtSet);
            _dataAdapter.Dispose();

            _dvOldRelease = new DataView(_dtSet);   // Must keep these seperate so combo's can select independently
            _dvNewRelease = new DataView(_dtSet);
        }

        private void PopulateDB()
        {

            // Populate DB Combo
            //DataView viewDB = new DataView(_dsMain.Tables["Set"]);
            _dtDBName = (new DataView(_dtSet)).ToTable(true, "DBName");
            //DataTable distinct = viewDB.ToTable(true, "DBName");

            this.comboDB.Items.Clear();
            this.comboDB.Items.Add("");
//            this.comboDB.Items.Add("<Big 5>");

            for (int i = 0; i < _dtDBName.Rows.Count; i++)
                this.comboDB.Items.Add(_dtDBName.Rows[i][0].ToString());

            _dvOldRelease.RowFilter = "DBName = ''";
            _dvNewRelease.RowFilter = "DBName = ''";
        }

        private void comboDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshRelPatch("O");
        }

        private void RefreshRelPatch(string OldOrNew)
        {
            string OldRelease, OldRevision, NewRelease, NewRevision, NewFilterInfo, OldRel, OldPat;

            switch (OldOrNew)
            {
                case "O":
                    OldRelease = this.comboOldRelease.Text;
                    OldRevision = this.comboOldRevision.Text;
                    NewRelease = this.comboNewRelease.Text;
                    NewRevision = this.comboNewRevision.Text;

                    if (comboDB.Text.Length > 5)
                    {
                        // Got a good value, proceed
                        _dvOldRelease.RowFilter = "DBName = '" + comboDB.Text + "'";
                        _dvNewRelease.RowFilter = "DBName = '" + comboDB.Text + "'";

                        _dtCurOld = _dvOldRelease.ToTable(true, "RelPatch");
                        _dtCurNew = _dvNewRelease.ToTable(true, "RelPatch");

                        this.comboOldRelease.DataSource = _dtCurOld;
                        this.comboOldRelease.DisplayMember = "RelPatch";

                        this.comboNewRelease.DataSource = _dtCurNew;
                        this.comboNewRelease.DisplayMember = "RelPatch";
                    }

                    this.comboOldRelease.Text = OldRelease;
                    this.comboOldRevision.Text = OldRevision;
                    this.comboNewRelease.Text = NewRelease;
                    this.comboNewRevision.Text = NewRevision;
                    break;
                case "N":
                    if (comboOldRevision.Text.Length > 0)
                    {
                        OldRel = comboOldRelease.Text.Substring(0, comboOldRelease.Text.IndexOf('.'));
                        OldPat = comboOldRelease.Text.Substring(comboOldRelease.Text.IndexOf('.') + 1);
                        NewFilterInfo = "DBName = '" + comboDB.Text + "'";
                        NewFilterInfo += " AND ((Release > " + OldRel + ")";
                        NewFilterInfo += "  OR (Release = " + OldRel + " AND Patch > " + OldPat + ")";
                        NewFilterInfo += "  OR (Release = " + OldRel + " AND Patch = " + OldPat + " AND Revision > " + comboOldRevision.Text + "))";
                        _dvNewRelease.RowFilter = NewFilterInfo;

                        _dtCurNew = _dvNewRelease.ToTable(true, "RelPatch");
                        this.comboNewRelease.DataSource = _dtCurNew;
                    }

                    break;
            }
        }


        private void comboOldRelease_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshRevision("O");
            RefreshRelPatch("N");   // update the release/patch/revision for new release
        }

        private void comboNewRelease_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshRevision("N");
        }

        private void comboOldRevision_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshRelPatch("N");   // update the release/patch/revision for new release
        }

        private void RefreshRevision(string OldOrNew)
        {
            switch (OldOrNew) {
                case "O":
                    if (comboOldRelease.Text != null)
                    {
                        _dvOldRelease.RowFilter = "DBName = '" + comboDB.Text + "' AND RelPatch = '" + comboOldRelease.Text + "'";

                        this.comboOldRevision.DataSource = _dvOldRelease;
                        this.comboOldRevision.ValueMember = "SetID";
                        this.comboOldRevision.DisplayMember = "Revision";

                        if (this.comboOldRevision.Items.Count > 0)
                            this.comboOldRevision.SelectedIndex = 0;
                    }
                    break;
                case "N":
                    if (comboOldRelease.Text != null)
                    {
                        _dvNewRelease.RowFilter = "DBName = '" + comboDB.Text + "' AND RelPatch = '" + comboNewRelease.Text + "'";

                        if (comboOldRelease.Text.Equals(comboNewRelease.Text))
                            _dvNewRelease.RowFilter += " AND Revision > " + comboOldRevision.Text;

                        this.comboNewRevision.DataSource = _dvNewRelease;
                        this.comboNewRevision.ValueMember = "SetID";
                        this.comboNewRevision.DisplayMember = "Revision";

                        if (this.comboNewRevision.Items.Count > 0)
                            this.comboNewRevision.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private void SchemaMaster_Load(object sender, EventArgs e)
        {
            GetSetData();
            PopulateDB();
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            _dataAdapter = new SqlDataAdapter();

            _resultsTitle = this.comboOldRelease.Text + "." + this.comboOldRevision.SelectedIndex + " (SetID: " + this.comboOldRevision.SelectedValue + ")  --> " + this.comboNewRelease.Text + "." + this.comboNewRevision.SelectedIndex + " (SetID: " + this.comboNewRevision.SelectedValue + ")";
                
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = _conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DataDictionary.x_GetDifferences";
                cmd.Parameters.Add(new SqlParameter("@OldSetID", comboOldRevision.SelectedValue));
                cmd.Parameters.Add(new SqlParameter("@NewSetID", comboNewRevision.SelectedValue));
                _dataAdapter.SelectCommand = cmd;
            
                _dsResults = new DataSet();

                _dataAdapter.Fill(_dsResults);
            }

            Results formResults = new Results(_dsResults, _resultsTitle);
            formResults.ShowDialog();

            _dataAdapter.Dispose();
            _dsResults.Dispose();
        }


    }
}
