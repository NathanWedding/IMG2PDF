using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IMG2PDF {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            // fill controls from settings
            rotateCheckBox.Checked = settings.Default.Rotate;
            sourcePathTextBox.Text = settings.Default.File;
            sizeComboBox.SelectedIndex = sizeComboBox.Items.IndexOf(settings.Default.Size);
            sortComboBox.SelectedIndex = sortComboBox.Items.IndexOf(settings.Default.Sort);
            marginBoxLeft.Text = settings.Default.Margins.Left.ToString();
            marginBoxTop.Text = settings.Default.Margins.Top.ToString();
            marginBoxRight.Text = settings.Default.Margins.Right.ToString();
            marginBoxBottom.Text = settings.Default.Margins.Bottom.ToString();
        }

        // browse for path
        private void BrowseButton_Click(object sender, EventArgs e) {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if(FBD.ShowDialog() == DialogResult.OK) {
                sourcePathTextBox.Text = System.IO.Path.GetFullPath(FBD.SelectedPath);
            }
        }

        private void PDFButton_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog {
                Filter = "PDF files (*.pdf)|*.pdf"
            };
            if(sfd.ShowDialog() == DialogResult.OK) {
                string pdfPath = System.IO.Path.GetFullPath(sfd.FileName);
                bool rotate = rotateCheckBox.Checked;
                if(!Directory.Exists(sourcePathTextBox.Text)) {
                    throw new Exception("Source Image folder does not exist");
                }
                List<string> fileList = Directory.GetFiles(sourcePathTextBox.Text).OrderBy(f => f).ToList();
                foreach(string file in fileList) {
                    switch(System.IO.Path.GetExtension(file)) {
                        case ".png":
                        case ".jpg":
                        case ".tif":
                        case ".gif":
                            break;
                        default:
                            fileList.Remove(file);
                            break;
                    }
                }
                string[] files = fileList.ToArray();
                if(files.Length == 0) {
                    throw new Exception("Source Image folder does not contain any images");
                }
                switch(sortComboBox.Text) {
                    case "Leading Number":
                        SortByLeadingNum(ref files);
                        break;
                    default:
                        break;
                }

                // pdf writer and document
                PdfWriter pdfWriter = new PdfWriter(pdfPath, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
                PdfDocument pdfDoc = new PdfDocument(pdfWriter);

                // set page size
                PageSize pageSize = null;
                switch(sizeComboBox.Text) {
                    case "A0":
                        pageSize = PageSize.A0;
                        break;
                    case "A1":
                        pageSize = PageSize.A1;
                        break;
                    case "A2":
                        pageSize = PageSize.A2;
                        break;
                    case "A3":
                        pageSize = PageSize.A3;
                        break;
                    case "A4":
                        pageSize = PageSize.A4;
                        break;
                    case "A5":
                        pageSize = PageSize.A5;
                        break;
                    case "A6":
                        pageSize = PageSize.A6;
                        break;
                    case "A7":
                        pageSize = PageSize.A7;
                        break;
                    case "A8":
                        pageSize = PageSize.A8;
                        break;
                    default:
                        pageSize = PageSize.A4;
                        break;
                }

                pdfDoc.SetDefaultPageSize(pageSize);

                // set writable doc
                Document doc = new Document(pdfDoc);

                // set margins

                /*
                doc.SetMargins(int.Parse(marginBoxTop.Text), int.Parse(marginBoxRight.Text),
                    int.Parse(marginBoxBottom.Text), int.Parse(marginBoxLeft.Text));
                */

                for(int i = 0; i < files.Length; i++) {
                    Image img = new Image(ImageDataFactory.Create(files[i]));
                    float widthRatio, heightRatio;
                    // TODO option for this, take margins into account
                    if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                        img.SetRotationAngle(90 * Math.PI / 180f);
                        widthRatio = pdfDoc.GetDefaultPageSize().GetWidth() / img.GetImageHeight();
                        heightRatio = pdfDoc.GetDefaultPageSize().GetHeight() / img.GetImageWidth();
                    }
                    else {
                        widthRatio = pdfDoc.GetDefaultPageSize().GetWidth() / img.GetImageWidth();
                        heightRatio = pdfDoc.GetDefaultPageSize().GetHeight() / img.GetImageHeight();
                    }
                    float ratio = widthRatio < heightRatio ? widthRatio : heightRatio;
                    img.ScaleAbsolute(img.GetImageWidth() * ratio, img.GetImageHeight() * ratio);
                    if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                        img.SetMargins(
                            (pdfDoc.GetDefaultPageSize().GetHeight() - img.GetImageScaledWidth()) / 2f,
                            (pdfDoc.GetDefaultPageSize().GetWidth() - img.GetImageScaledHeight()) / 2f,
                            (pdfDoc.GetDefaultPageSize().GetHeight() - img.GetImageScaledWidth()) / 2f,
                            (pdfDoc.GetDefaultPageSize().GetWidth() - img.GetImageScaledHeight()) / 2f
                        );
                    }
                    else {
                        img.SetMargins(
                            (pdfDoc.GetDefaultPageSize().GetHeight() - img.GetImageScaledHeight()) / 2f,
                            (pdfDoc.GetDefaultPageSize().GetWidth() - img.GetImageScaledWidth()) / 2f,
                            (pdfDoc.GetDefaultPageSize().GetHeight() - img.GetImageScaledHeight()) / 2f,
                            (pdfDoc.GetDefaultPageSize().GetWidth() - img.GetImageScaledWidth()) / 2f
                        );
                    }
                    doc.Add(img);
                    if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                        pdfDoc.GetPage(pdfDoc.GetNumberOfPages()).SetRotation(90);
                    }
                }
                doc.Close();
                pdfDoc.Close();
                settings.Default.File = sourcePathTextBox.Text;
                settings.Default.Rotate = rotate;
                settings.Default.Size = sizeComboBox.Text;
                settings.Default.Sort = sortComboBox.Text;
                settings.Default.Save();
                MessageBox.Show(this, "file successfully created");
            }
        }

        //TODO error handling
        private void StartButton_Click(object sender, EventArgs e) {
            try {
                SaveFileDialog SFD = new SaveFileDialog {
                    Filter = "PDF files (*.pdf)|*.pdf"
                };
                if(SFD.ShowDialog() == DialogResult.OK) {
                    string pdfPath = System.IO.Path.GetFullPath(SFD.FileName);
                    bool rotate = rotateCheckBox.Checked;
                    if(!Directory.Exists(sourcePathTextBox.Text)) {
                        throw new Exception("Source Image folder does not exist");
                    }
                    List<string> fileList = Directory.GetFiles(sourcePathTextBox.Text).OrderBy(f => f).ToList();
                    foreach(string file in fileList) {
                        switch(System.IO.Path.GetExtension(file)) {
                            case ".png":
                            case ".jpg":
                            case ".tif":
                            case ".gif":
                                break;
                            default:
                                fileList.Remove(file);
                                break;
                        }
                    }
                    string[] files = fileList.ToArray();
                    if(files.Length == 0) {
                        throw new Exception("Source Image folder does not contain any images");
                    }
                    switch(sortComboBox.Text) {
                        case "Leading Number":
                            SortByLeadingNum(ref files);
                            break;
                        default:
                            break;
                    }

                    // pdf writer and document
                    PdfWriter pdfWriter = new PdfWriter(pdfPath, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
                    PdfDocument pdfDoc = new PdfDocument(pdfWriter);

                    // set page size
                    PageSize pageSize = null;
                    switch(sizeComboBox.Text) {
                        case "A0":
                            pageSize = PageSize.A0;
                            break;
                        case "A1":
                            pageSize = PageSize.A1;
                            break;
                        case "A2":
                            pageSize = PageSize.A2;
                            break;
                        case "A3":
                            pageSize = PageSize.A3;
                            break;
                        case "A4":
                            pageSize = PageSize.A4;
                            break;
                        case "A5":
                            pageSize = PageSize.A5;
                            break;
                        case "A6":
                            pageSize = PageSize.A6;
                            break;
                        case "A7":
                            pageSize = PageSize.A7;
                            break;
                        case "A8":
                            pageSize = PageSize.A8;
                            break;
                        default:
                            pageSize = PageSize.A4;
                            break;
                    }

                    pdfDoc.SetDefaultPageSize(pageSize);

                    // set writable doc
                    Document doc = new Document(pdfDoc);
                    // set margins
                    if(!int.TryParse(marginBoxTop.Text, out int topMargin) || 
                        !int.TryParse(marginBoxRight.Text, out int rightMargin) || 
                        !int.TryParse(marginBoxBottom.Text, out int bottomMargin) ||
                        !int.TryParse(marginBoxLeft.Text, out int leftMargin)) {
                        throw new Exception("Invalid margins");
                    }
                    doc.SetMargins(topMargin, rightMargin, bottomMargin, leftMargin);

                    for(int i = 0; i < files.Length; i++) {
                        Image img = new Image(ImageDataFactory.Create(files[i]));
                        float widthRatio, heightRatio;
                        // TODO option for this, take margins into account
                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            img.SetRotationAngle(90 * Math.PI / 180f);
                            widthRatio = pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin / img.GetImageHeight();
                            heightRatio = pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin / img.GetImageWidth();
                        }
                        else {
                            widthRatio = pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin / img.GetImageWidth();
                            heightRatio = pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin / img.GetImageHeight();
                        }
                        float ratio = widthRatio < heightRatio ? widthRatio : heightRatio;
                        img.ScaleAbsolute(img.GetImageWidth() * ratio, img.GetImageHeight() * ratio);
                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            img.SetMargins(
                                (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin - img.GetImageScaledWidth()) / 2f,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin - img.GetImageScaledHeight()) / 2f,
                                (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin - img.GetImageScaledWidth()) / 2f,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin - img.GetImageScaledHeight()) / 2f
                            );
                        }
                        else {
                            img.SetMargins(
                                (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin - img.GetImageScaledHeight()) / 2f,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin - img.GetImageScaledWidth()) / 2f,
                                (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin - img.GetImageScaledHeight()) / 2f,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin - img.GetImageScaledWidth()) / 2f
                            );
                        }
                        doc.Add(img);
                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            pdfDoc.GetPage(pdfDoc.GetNumberOfPages()).SetRotation(90);
                        }
                    }
                    doc.Close();
                    pdfDoc.Close();
                    settings.Default.File = sourcePathTextBox.Text;
                    settings.Default.Rotate = rotate;
                    settings.Default.Size = sizeComboBox.Text;
                    settings.Default.Sort = sortComboBox.Text;
                    settings.Default.Margins = new Padding(leftMargin, topMargin, rightMargin, bottomMargin);
                    settings.Default.Save();
                    outputLabel.Text = "file successfully created";
                }
            }
            catch(Exception ex) {
                outputLabel.Text = ex.Message;
            }
        }

        // sorts a list of strings by the leading number in the string
        //TODO error handling
        private void SortByLeadingNum(ref string[] list) {
            int[] ints = new int[list.Length];
            for(int i = 0; i < list.Length; i++) {
                string sNum = "";
                for(int c = 0; c < list[i].Length; c++) {
                    string a = System.IO.Path.GetFileName(list[i]);
                    if(char.IsDigit(a[c])) {
                        sNum += a[c];
                    }
                    else {
                        break;
                    }
                }
                try {
                    ints[i] = int.Parse(sNum);
                }
                catch(Exception) {
                    throw new Exception("Source Image named incorrectly: " + System.IO.Path.GetFileName(list[i]));
                }
            }

            int tempInt;
            string tempString;

            for(int j = list.Length - 1; j > 0; j--) {
                for(int i = 0; i < j; i++) {
                    if(ints[i] > ints[i + 1]) {
                        tempInt = ints[i + 1];
                        tempString = list[i + 1];
                        ints[i + 1] = ints[i];
                        list[i + 1] = list[i];
                        ints[i] = tempInt;
                        list[i] = tempString;
                    }
                }
            }
        }
    }
}
