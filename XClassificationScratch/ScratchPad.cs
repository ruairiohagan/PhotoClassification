using System.Net;
using ClassificationAPI;

namespace XClassificationScratch
{
    /// <summary>
    /// Scratch form for exploritave work
    /// </summary>
    public partial class ScratchPad : Form
    {
        public ScratchPad()
        {
            InitializeComponent();
        }

        private void btnFileSelector_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = false;
                ofd.Filter = "JPGs (*.jpg)|*.jpg";
                DialogResult iRes = ofd.ShowDialog();
                if (iRes == DialogResult.OK)
                {
                    txtImageFile.Text = ofd.FileName;
                    pbImage.ImageLocation = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnClassify_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtImageFile.Text == "")
                {
                    MessageBox.Show("Select an image to classify");
                    return;
                }
                Task.Factory.StartNew(async () =>
                {
                    ClassificationEventArgs e = await LMStudio.ClassifyImage(txtAPILocation.Text, txtModelName.Text, txtQuestion.Text, txtImageFile.Text, 30000);

                    if (e.resultCode == HttpStatusCode.OK)
                    {
                        txtResponse.Invoke(new Action(() => txtResponse.Text = e.description));
                    }
                    else
                    {
                        MessageBox.Show(e.description, $"HTTP Error {e.resultCode}!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
