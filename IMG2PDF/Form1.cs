using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IMG2PDF {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            // fill controls from settings
            //TODO sort out if the user messes with the settings file
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

        // generate and save pdf
        private void SaveButton_Click(object sender, EventArgs e) {
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
                    // get list of image paths, remove anything that isn't an image
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
                    // throw if there are no images
                    if(files.Length == 0) {
                        throw new Exception("Source Image folder does not contain any images");
                    }
                    switch(sortComboBox.Text) {
                        case "Leading Number":
                            SortByLeadingNum(ref files);
                            break;
                        default:
                            break; // sorted alphabetically by default
                    }

                    // pdf writer and document init
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
                    float pageWidth = pdfDoc.GetDefaultPageSize().GetWidth();
                    float pageHeight = pdfDoc.GetDefaultPageSize().GetHeight();

                    // set writable doc
                    Document doc = new Document(pdfDoc);

                    // set document margins to zero
                    doc.SetMargins(0, 0, 0, 0);

                    //TODO do this on a different thread
                    for(int i = 0; i < files.Length; i++) {
                        // create image object
                        Bitmap sourceBitmap = new Bitmap(files[i]);

                        // calculate ratios for resizing, based on whether to rotate or not
                        float widthRatio, heightRatio;
                        if(rotate && sourceBitmap.Width > sourceBitmap.Height) {
                            widthRatio = (pageHeight - leftMargin - rightMargin ) / sourceBitmap.Width;
                            heightRatio = (pageWidth - topMargin - bottomMargin) / sourceBitmap.Height;
                        }
                        else {
                            widthRatio = (pageWidth - leftMargin - rightMargin) / sourceBitmap.Width;
                            heightRatio = (pageHeight - topMargin - bottomMargin) / sourceBitmap.Height;
                        }
                        // if margins are too big, throw exception
                        if(widthRatio < 0 || heightRatio < 0) {
                            throw new Exception("Image " + System.IO.Path.GetFileName(files[i]) + " cannot fit within the margins.");
                        }
                        // scale by smaller ratio and convert to new bitmap without losing quality
                        float ratio = widthRatio < heightRatio ? widthRatio : heightRatio;
                        Bitmap scaledBitmap = new Bitmap((int)(sourceBitmap.Width * ratio), (int)(sourceBitmap.Height * ratio));
                        scaledBitmap.SetResolution(sourceBitmap.HorizontalResolution, sourceBitmap.VerticalResolution);
                        Graphics graphic = Graphics.FromImage(scaledBitmap);
                        // TODO quality settings
                        graphic.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                        graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        ImageAttributes attributes = new ImageAttributes();
                        attributes.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                        graphic.DrawImage(sourceBitmap, new System.Drawing.Rectangle(0, 0, scaledBitmap.Width, scaledBitmap.Height),
                            0, 0, sourceBitmap.Width, sourceBitmap.Height, GraphicsUnit.Pixel, attributes);

                        // convert to byte array, then to itext image
                        MemoryStream stream = new MemoryStream();
                        // TODO change which image format is used
                        scaledBitmap.Save(stream, ImageFormat.Jpeg);
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(stream.ToArray()));

                        // rotate and set margins
                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            img.SetRotationAngle(90 * Math.PI / 180f);
                            img.SetMargins(
                                (pageHeight - rightMargin - leftMargin - img.GetImageWidth()) / 2 + rightMargin,
                                (pageWidth - bottomMargin - topMargin - img.GetImageHeight()) / 2 + bottomMargin,
                                (pageHeight - leftMargin - rightMargin - img.GetImageWidth()) / 2 + leftMargin,
                                (pageWidth - topMargin - bottomMargin - img.GetImageHeight()) / 2 + topMargin
                            );
                        }
                        else {
                            img.SetMargins(
                                (pageHeight - topMargin - bottomMargin - img.GetImageHeight()) / 2f + topMargin,
                                (pageWidth - rightMargin - leftMargin - img.GetImageWidth()) / 2f + rightMargin,
                                (pageHeight - bottomMargin - topMargin - img.GetImageHeight()) / 2f + bottomMargin,
                                (pageWidth - leftMargin - rightMargin - img.GetImageWidth()) / 2f + leftMargin
                            );
                        }



                        // add picture to document
                        doc.Add(img);
                        if(rotate && img.GetImageWidth() > img.GetImageHeight()) {
                            pdfDoc.GetPage(pdfDoc.GetNumberOfPages()).SetRotation(90);
                        }
                    }

                    // close docs and save settings
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

        // sorts a list of path strings by the leading number in the filename
        private void SortByLeadingNum(ref string[] list) {
            int[] ints = new int[list.Length];
            // retrieve number
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
            // sort
            // TODO more efficient sort?
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
