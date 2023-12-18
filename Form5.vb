Imports System.Data.Odbc
Imports System.Globalization
Imports System.Drawing.Printing
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form5
    Dim conn As OdbcConnection
    Dim Da As OdbcDataAdapter
    Dim Ds As DataSet
    Dim Cmd As OdbcCommand
    Dim Rd As OdbcDataReader
    Dim MyDB As String
    Dim re As New BindingSource
    Dim WithEvents PD As New PrintDocument
    Dim PPD As New PrintPreviewDialog
    Dim longpaper As Integer
    Sub koneksi()
        MyDB = "Driver={Mysql ODBC 3.51 Driver};Database=warteg_ajah;Server=localhost;uid=root"
        conn = New OdbcConnection(MyDB)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
    End Sub
    Sub ketemu()
        On Error Resume Next
        ComboBox1.Text = Rd(1) '=
        TextBox2.Text = Rd(2) '=
        ComboBox2.Text = Rd(3) '=
        TextBox4.Text = Rd(4) '=
        TextBox5.Text = Rd(5) '=
        TextBox6.Text = Rd(6) '=
    End Sub
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""
        Call koneksi()
        Da = New OdbcDataAdapter("Select * from keranjang", conn)
        Ds = New DataSet
        Da.Fill(Ds, "keranjang")
        DataGridView1.DataSource = Ds.Tables("keranjang")
    End Sub
    Sub simpandata()
        Call koneksi()
        Da = New Odbc.OdbcDataAdapter("select * from data_menu ", conn)
        Ds = New DataSet
        Da.Fill(Ds)
        re.DataSource = Ds
        re.DataMember = Ds.Tables(0).ToString
    End Sub
    Sub tampilgrid()
        Call koneksi()
        Da = New OdbcDataAdapter("select*from keranjang ORDER BY Id_Transaksi DESC", conn)
        Ds = New DataSet
        Da.Fill(Ds, "keranjang")
        DataGridView1.DataSource = Ds.Tables("keranjang")
        'responsive dg
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
    Sub itemcomboo()
        Call koneksi()
        Da = New Odbc.OdbcDataAdapter("select * from data_menu ", conn)
        Ds = New DataSet
        Da.Fill(Ds)
        re.DataSource = Ds
        re.DataMember = Ds.Tables(0).ToString
        Dim a As DataRow
        ComboBox1.Items.Clear()
        For Each a In Ds.Tables(0).Rows
            ComboBox1.Items.Add(a.Item(0))
        Next a
    End Sub

    Sub hitung()
        Dim sum As Integer = 0
        For i = 0 To DataGridView1.Rows.Count - 1
            sum += DataGridView1.Rows(i).Cells(6).Value
        Next
        TextBox7.Text = sum
    End Sub


    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        Call KondisiAwal()
        Call simpandata()
        Call itemcomboo()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try

            If TextBox3.Text = "" Or ComboBox1.Text = "" Or TextBox2.Text = "" Or ComboBox2.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Or TextBox6.Text = "" Then
                MsgBox("Data harus lengkap", vbExclamation, "Pesan")
            Else
                Cmd = New OdbcCommand("select * from keranjang where Id_Transaksi = '" & TextBox3.Text & "'", conn)
                Rd = Cmd.ExecuteReader
                Rd.Read()
                If Not Rd.HasRows Then
                    Dim simpan As String = "INSERT INTO keranjang (Id_Transaksi, Id_Produk, Nama_Produk, Kategori, Harga_Satuan, Jumlah, Total, Tanggal) VALUES ('" & TextBox3.Text & "','" & ComboBox1.Text & "','" & TextBox2.Text & "', '" & ComboBox2.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "','" & TextBox6.Text & "','" & DateTimePicker1.Value.ToString("yyyy-MM-dd") & "')"

                    Cmd = New OdbcCommand(simpan, conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Data berhasil di simpan", vbInformation, "Simpan")
                    Call KondisiAwal()
                Else
                    MsgBox("data sudah ada")
                    TextBox3.Focus()
                End If
            End If
        Catch ex As Exception
            MsgBox("Terdapat kesalahan" & ex.Message)
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Call koneksi()
        Cmd = New OdbcCommand("select * from data_menu where Id_Produk='" & ComboBox1.Text & "'", conn)
        Rd = Cmd.ExecuteReader
        Rd.Read()
        TextBox2.Text = Rd.Item(1) '=nama
        ComboBox2.Text = Rd.Item(2) '=perusahaan
        TextBox4.Text = Rd.Item(3) '=id_barang
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox6.Text = Val(TextBox4.Text) * Val(TextBox5.Text)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If MessageBox.Show("Apakah anda yakin ingin menghapus data ?", "info", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Call koneksi()
            Dim hapusdata As String = "Delete from keranjang where Id_Transaksi = '" & TextBox3.Text & "'"
            Cmd = New OdbcCommand(hapusdata, conn)
            Cmd.ExecuteReader()
            Call KondisiAwal()
            MsgBox("Data berhasil dihapus", MsgBoxStyle.Information, "Information")

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim i As Integer
        i = Me.DataGridView1.CurrentRow.Index
        With DataGridView1.Rows.Item(i)
            Me.TextBox3.Text = .Cells(0).Value
            Me.ComboBox1.Text = .Cells(1).Value
            Me.TextBox2.Text = .Cells(2).Value
            Me.ComboBox2.Text = .Cells(3).Value
            Me.TextBox4.Text = .Cells(4).Value
            Me.TextBox5.Text = .Cells(5).Value
            Me.TextBox6.Text = .Cells(6).Value
        End With
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Call hitung()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim angka1 As Integer = Integer.Parse(TextBox7.Text)
        Dim angka2 As Integer = Integer.Parse(TextBox1.Text)

        If angka1 > angka2 Then
            MsgBox("nominal tidak cukup")
            TextBox1.Text = ""
        Else
            Dim hasil As Integer = angka2 - angka1
            TextBox8.Text = hasil.ToString
        End If
    End Sub
    Private Sub PD_BeginPrint(sender As Object, e As PrintEventArgs) Handles PD.BeginPrint
        'Dim pagesetup As New PageSettings
        'pagesetup.PaperSize = New PaperSize("Custom", 250, 500) 'fixed size
        Dim papersize As New PaperSize("Custom", 300, 500)
        'pagesetup.PaperSize = New PaperSize("Custom", 250, longpaper)
        PD.DefaultPageSettings.PaperSize = papersize
    End Sub
    Sub changelongpaper()
        Dim rowcount As Integer
        longpaper = 0
        rowcount = DataGridView1.Rows.Count
        longpaper = rowcount * 15
        longpaper = longpaper + 240
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        changelongpaper()
        PPD.Document = PD
        PPD.ShowDialog()
    End Sub

    Private Sub PD_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PD.PrintPage
        Dim f10 As New Font("Times New Roman", 10, FontStyle.Regular)
        Dim f10b As New Font("Times New Roman", 10, FontStyle.Bold)
        Dim f14 As New Font("Times New Roman", 14, FontStyle.Bold)

        Dim leftmargin As Integer = PD.DefaultPageSettings.Margins.Left
        Dim centermargin As Integer = PD.DefaultPageSettings.PaperSize.Width / 2
        Dim rightmargin As Integer = PD.DefaultPageSettings.PaperSize.Width

        Dim kanan As New StringFormat
        Dim tengah As New StringFormat
        kanan.Alignment = StringAlignment.Far
        tengah.Alignment = StringAlignment.Center

        Dim garis As String
        garis = "------------------------------------------------------------------"

        e.Graphics.DrawString("Warteg Mamoka Bahari", f14, Brushes.Black, centermargin, 5, tengah)
        e.Graphics.DrawString("JL.Trias Estate, Cibitung, Bekasi," & vbNewLine & " Kab Bekasi, Jawa Barat, 14500", f10, Brushes.Black, centermargin, 30, tengah)
        e.Graphics.DrawString("Hp: 0812-9436-7108", f10, Brushes.Black, centermargin, 65, tengah)



        e.Graphics.DrawString(Date.Now(), f10, Brushes.Black, 0, 105)
        e.Graphics.DrawString("Nama Produk", f10, Brushes.Black, 0, 125)
        e.Graphics.DrawString("Kategori", f10, Brushes.Black, 100, 125)
        e.Graphics.DrawString("Harga", f10, Brushes.Black, 170, 125)
        e.Graphics.DrawString("Qty", f10, Brushes.Black, 220, 125)
        e.Graphics.DrawString("Total", f10, Brushes.Black, rightmargin, 125, kanan)
        e.Graphics.DrawString(garis, f10, Brushes.Black, 0, 130)
        Dim tinggi As Integer
        Dim total As Integer
        For Each baris As DataGridViewRow In DataGridView1.Rows
            If Not baris.IsNewRow Then
                tinggi += 15
                e.Graphics.DrawString(baris.Cells(2).Value, f10, Brushes.Black, 0, 130 + tinggi)
                e.Graphics.DrawString(baris.Cells(3).Value, f10, Brushes.Black, 100, 130 + tinggi)
                e.Graphics.DrawString(baris.Cells(4).Value, f10, Brushes.Black, 170, 130 + tinggi)
                e.Graphics.DrawString(baris.Cells(5).Value, f10, Brushes.Black, 220, 130 + tinggi)
                e.Graphics.DrawString(baris.Cells(6).Value, f10, Brushes.Black, rightmargin, 130 + tinggi, kanan)
                total += CDbl(baris.Cells(6).Value)
            End If
        Next
        tinggi = 140 + tinggi
        e.Graphics.DrawString(garis, f10, Brushes.Black, 0, tinggi)
        e.Graphics.DrawString("Subtotal :" & FormatCurrency(total), f10b, Brushes.Black, 150, 15 + tinggi)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If MessageBox.Show("Apakah Anda Ingin Menyelesaikan Transaksi ?", "info", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Call koneksi()
            Dim hapusdata As String = "Delete from keranjang"
            Cmd = New OdbcCommand(hapusdata, conn)
            Cmd.ExecuteReader()
            Call KondisiAwal()
            MsgBox("Transaksi Selesai", MsgBoxStyle.Information, "Information")
        End If

    End Sub
End Class