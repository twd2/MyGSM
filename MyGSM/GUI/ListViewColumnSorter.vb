Imports System.Collections
Imports System.Windows.Forms

''' <summary>
''' 继承自IComparer
''' </summary>
Public Class ListViewColumnSorter
    Implements IComparer
    ''' <summary>
    ''' 指定按照哪个列排序
    ''' </summary>
    Private ColumnToSort As Integer
    ''' <summary>
    ''' 指定排序的方式
    ''' </summary>
    Private OrderOfSort As SortOrder
    ''' <summary>
    ''' 声明CaseInsensitiveComparer类对象，
    ''' 参见ms-help://MS.VSCC.2003/MS.MSDNQTR.2003FEB.2052/cpref/html/frlrfSystemCollectionsCaseInsensitiveComparerClassTopic.htm
    ''' </summary>
    Private ObjectCompare As CaseInsensitiveComparer
    ''' <summary>
    ''' 构造函数
    ''' </summary>
    Public Sub New()
        ' 默认按第一列排序
        ColumnToSort = 0

        ' 排序方式为不排序
        OrderOfSort = SortOrder.None

        ' 初始化CaseInsensitiveComparer类对象
        ObjectCompare = New CaseInsensitiveComparer()
    End Sub

    ''' <summary>
    ''' 重写IComparer接口.
    ''' </summary>
    ''' <param name="x">要比较的第一个对象</param>
    ''' <param name="y">要比较的第二个对象</param>
    ''' <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
    Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim compareResult As Integer
        Dim listviewX, listviewY As String

        ' 将比较对象转换为ListViewItem对象
        listviewX = DirectCast(x, ListViewItem).SubItems(ColumnToSort).Text
        listviewY = DirectCast(y, ListViewItem).SubItems(ColumnToSort).Text

        ' 比较
        If ColumnToSort = 1 Then
            compareResult = IIf(Date.Parse(listviewX) > Date.Parse(listviewY), 1, -1)
        Else
            compareResult = ObjectCompare.Compare(listviewX, listviewY)
        End If

        ' 根据上面的比较结果返回正确的比较结果
        If OrderOfSort = SortOrder.Ascending Then
            ' 因为是正序排序，所以直接返回结果
            Return compareResult
        ElseIf OrderOfSort = SortOrder.Descending Then
            ' 如果是反序排序，所以要取负值再返回
            Return (-compareResult)
        Else
            ' 如果相等返回0
            Return 0
        End If
    End Function

    ''' <summary>
    ''' 获取或设置按照哪一列排序.
    ''' </summary>
    Public Property SortColumn() As Integer
        Get
            Return ColumnToSort
        End Get
        Set(ByVal value As Integer)
            ColumnToSort = value
        End Set
    End Property

    ''' <summary>
    ''' 获取或设置排序方式.
    ''' </summary>
    Public Property Order() As SortOrder
        Get
            Return OrderOfSort
        End Get
        Set(ByVal value As SortOrder)
            OrderOfSort = value
        End Set
    End Property

End Class