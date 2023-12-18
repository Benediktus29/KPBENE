Imports System.Data.Odbc
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form3
    Dim conn As OdbcConnection
    Dim Da As OdbcDataAdapter
    Dim Ds As DataSet
    Dim Cmd As OdbcCommand
    Dim Rd As OdbcDataReader
    Dim MyDB As String

    Sub koneksi()
        MyDB = "Driver={Mysql ODBC 3.51 Driver};Database=warteg_ajah;Server=localhost;uid=root"
        conn = New OdbcConnection(MyDB)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
    End Sub
    Sub ketemu()
        On Error Resume Next
        TextBox2.Text = Rd(1) '=
        ComboBox1.Text = Rd(2) '=
        TextBox4.Text = Rd(3) '=

    End Sub


    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        ComboBox1.Text = ""
        TextBox4.Text = ""
        Button1.Text = "INPUT"
        Button2.Text = "EDIT"
        Button3.Text = "DELETE"
        Button4.Text = "TUTUP"
        Call koneksi()
        Da = New OdbcDataAdapter("Select * from data_menu", conn)
        Ds = New DataSet
        Da.Fill(Ds, "data_menu")
        DataGridView1.DataSource = Ds.Tables("data_menu")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try

            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox4.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Data harus lengkap", vbExclamation, "Pesan")
            Else
                Cmd = New OdbcCommand("select * from data_menu where Id_Produk = '" & TextBox1.Text & "'", conn)
                Rd = Cmd.ExecuteReader
                Rd.Read()
                If Not Rd.HasRows Then
                    Dim simpan As String = "insert into data_menu (Id_Produk, Nama_Produk, Kategori, Harga_satuan) value ('" & TextBox1.Text & "','" & TextBox2.Text & "','" & ComboBox1.Text & "', '" & TextBox4.Text & "')"

                    Cmd = New OdbcCommand(simpan, conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Data berhasil di simpan", vbInformation, "Simpan")
                    Call KondisiAwal()
                Else
                    MsgBox("data sudah ada")
                    TextBox1.Focus()
                End If
            End If
        Catch ex As Exception
            MsgBox("Terdapat kesalahan" & ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Or ComboBox1.Text = "" Or TextBox4.Text = "" Then
            MsgBox("Data Belum Lengkap, Silahkan Isi Semua Field")
        Else
            Call koneksi()
            Dim EditData As String = "UPDATE data_menu SET Nama_Produk='" & TextBox2.Text & "', Kategori='" & ComboBox1.Text & "', Harga_satuan='" & TextBox4.Text & "' WHERE Id_Produk='" & TextBox1.Text & "'"
            Cmd = New OdbcCommand(EditData, conn)
            Cmd.ExecuteNonQuery()
            MsgBox("Data Berhasil Diedit", MsgBoxStyle.Information, "Informasi")
            Button2.Text = "Edit"
            Button1.Enabled = True
            Button3.Enabled = True
            Call KondisiAwal()
        End If
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim i As Integer
        i = Me.DataGridView1.CurrentRow.Index
        With DataGridView1.Rows.Item(i)
            Me.TextBox1.Text = .Cells(0).Value
            Me.TextBox2.Text = .Cells(1).Value
            Me.ComboBox1.Text = .Cells(2).Value
            Me.TextBox4.Text = .Cells(3).Value
        End With
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If MessageBox.Show("Apakah anda yakin ingin menghapus data ?", "info", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Call koneksi()
            Dim hapusdata As String = "Delete from data_menu where Id_Produk = '" & TextBox1.Text & "'"
            Cmd = New OdbcCommand(hapusdata, conn)
            Cmd.ExecuteReader()
            Call KondisiAwal()
            MsgBox("Data berhasil dihapus", MsgBoxStyle.Information, "Information")

        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
        Form2.Show()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class