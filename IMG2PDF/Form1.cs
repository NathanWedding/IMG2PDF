using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                try {
                    // set margins
                    if(!int.TryParse(marginBoxTop.Text, out int topMargin) ||
                        !int.TryParse(marginBoxRight.Text, out int rightMargin) ||
                        !int.TryParse(marginBoxBottom.Text, out int bottomMargin) ||
                        !int.TryParse(marginBoxLeft.Text, out int leftMargin)) {
                        throw new Exception("Invalid margins");
                    }

                    // get save path
                    string pdfPath = System.IO.Path.GetFullPath(sfd.FileName);
                    bool rotate = rotateCheckBox.Checked;
                    if(!Directory.Exists(sourcePathTextBox.Text)) {
                        throw new Exception("Source Image folder does not exist");
                    }
                    List<string> fileList = Directory.GetFiles(sourcePathTextBox.Text).OrderBy(f => f).ToList();
                    List<string> toRemove = null; ;
                    foreach(string file in fileList) {
                        switch(System.IO.Path.GetExtension(file)) {
                            case ".png":
                            case ".jpg":
                            case ".tif":
                            case ".gif":
                                break;
                            default:
                                if(toRemove == null) {
                                    toRemove = new List<string>();
                                }
                                toRemove.Add(file);
                                break;
                        }
                    }
                    if(toRemove != null) {
                        foreach(string file in toRemove) {
                            fileList.Remove(file);
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

                    // set margins to zero
                    doc.SetMargins(0, 0, 0, 0);

                    for(int i = 0; i < files.Length; i++) {
                        // create image object
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(files[i]));

                        // calculate ratios for resizing
                        float widthRatio, heightRatio;
                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            img.SetRotationAngle(90 * Math.PI / 180f);
                            widthRatio = (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin) / img.GetImageHeight();
                            heightRatio = (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin) / img.GetImageWidth();
                        }
                        else {
                            widthRatio = (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin) / img.GetImageWidth();
                            heightRatio = (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin) / img.GetImageHeight();
                        }
                        if(widthRatio < 0 || heightRatio < 0) {
                            throw new Exception("Image " + System.IO.Path.GetFileName(files[i]) + " cannot fit within the margins.");
                        }
                        float ratio = widthRatio < heightRatio ? widthRatio : heightRatio;
                        img.ScaleToFit(img.GetImageWidth() * ratio, img.GetImageHeight() * ratio);
                        Console.Write(
                            "\nIMAGE " + i + System.IO.Path.GetFileName(files[i]) +
                            "\nwidth:" + img.GetImageWidth() +
                            "\nheight:" + img.GetImageHeight() +
                            "\nscaled width:" + img.GetImageScaledWidth() +
                            "\nscaled height:" + img.GetImageScaledHeight()
                        );

                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            img.SetMargins(
                                (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin - img.GetImageScaledWidth()) / 2f + topMargin,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - rightMargin - leftMargin - img.GetImageScaledHeight()) / 2f + rightMargin,
                                (pdfDoc.GetDefaultPageSize().GetHeight() - bottomMargin - topMargin - img.GetImageScaledWidth()) / 2f + bottomMargin,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin - img.GetImageScaledHeight()) / 2f + leftMargin
                            );
                        }
                        else {
                            img.SetMargins(
                                (pdfDoc.GetDefaultPageSize().GetHeight() - topMargin - bottomMargin - img.GetImageScaledHeight()) / 2f + topMargin,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - rightMargin - leftMargin - img.GetImageScaledWidth()) / 2f + rightMargin,
                                (pdfDoc.GetDefaultPageSize().GetHeight() - bottomMargin - topMargin - img.GetImageScaledHeight()) / 2f + bottomMargin,
                                (pdfDoc.GetDefaultPageSize().GetWidth() - leftMargin - rightMargin - img.GetImageScaledWidth()) / 2f + leftMargin
                            );
                        }
                        Console.Write(
                            "\ntop margin:" + img.GetMarginTop() +
                            "\nbottom margin:" + img.GetMarginBottom() +
                            "\ntop margin:" + img.GetMarginLeft() +
                            "\nright margin:" + img.GetMarginRight()
                        );
                        doc.Add(img);
                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            pdfDoc.GetPage(pdfDoc.GetNumberOfPages()).SetRotation(90);
                        }
                        Console.Write("\n\n");
                    }
                    doc.Close();
                    pdfDoc.Close();
                    settings.Default.File = sourcePathTextBox.Text;
                    settings.Default.Rotate = rotate;
                    settings.Default.Size = sizeComboBox.Text;
                    settings.Default.Sort = sortComboBox.Text;
                    settings.Default.Margins = new Padding(leftMargin, topMargin, rightMargin, bottomMargin);
                    settings.Default.Save();
                    outputLabel.ForeColor = Color.Green;
                    outputLabel.Text = "file successfully created";
                }
                catch(Exception ex) {
                    outputLabel.ForeColor = Color.Red;
                    outputLabel.Text = ex.Message;
                }
            }
        }

        // sorts a list of strings by the leading number in the string
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
