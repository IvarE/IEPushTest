using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using CRM.Treeview.Category.Classes;
using CGIXrm;
using CGIXrm.CrmSdk;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;


// CRM.Treeview.CategoryPage.html?serveraddress=http://v-dkcrm-utv/Skanetrafiken&id=3C3E3895-CAFA-E311-80CF-0050569010AD&debug=yes

namespace CRM.Treeview.Category.ViewModel
{
    public class MainPageViewModel
    {
        //Private
        DataGrid _grdGrid;
        ListBox _lstListBox;
        string _id;
        string _caseStateCode = "0";
        string _debug = "";

        List<CategoryDetail> _originalCategoryDetails;
        List<ItemRow> _categoryitems;
        List<ItemRow> _sorted1;
        List<ItemRow> _sorted2;
        List<ItemRow> _newItems;
        List<SelectedCategory> _selectionList;
        List<ItemRow> _filteredList;

        SolidColorBrush brushDefault = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));

        /*
        14		FFD8CC	Röd
        90		E6FFCC	Grön
        230		CCD5FF	Blå
        65		FBFFCC	Gul		
        */

        SolidColorBrush brushA = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xD8, 0xCC)); //Röd     285050000
        SolidColorBrush brushB = new SolidColorBrush(Color.FromArgb(0xFF, 0xE6, 0xFF, 0xCC)); //Grön    285050001
        SolidColorBrush brushC = new SolidColorBrush(Color.FromArgb(0xFF, 0xCC, 0xD5, 0xFF)); //Blå     285050002
        SolidColorBrush brushD = new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xFF, 0xCC)); //Gul     285050003

        Brush brushCurrent;

        CrmManager _crmManager;
        Dispatcher _theDispather;
        WebParameters _webparameters;

        //Public
        
        public string Version
        {
            get { return System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[1].Split('=')[1].Trim(); }
        }

        public MainPageViewModel(DataGrid grdGrid, ListBox listbox, Dispatcher dispatcher, TextBlock textblock) 
        {
            try
            {
                textblock.Text = Version;
                _theDispather = dispatcher;
                _grdGrid = grdGrid;
                _lstListBox = listbox;
                _selectionList = new List<SelectedCategory>();
                _webparameters = new WebParameters();
                _crmManager = new CrmManager(_webparameters.ServerAddress);

                _debug = _getKey("debug");

                ////DEBUG
                //if (_webparameters != null && _webparameters.Id != null && _webparameters.Id != "")
                //    _id = _webparameters.Id;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string _getKey(string skey)
        {
            string _returnvalue = "";
            try
            {
                foreach (string _s in System.Windows.Browser.HtmlPage.Document.QueryString.Keys)
                {
                    if (_s.ToLower() == "debug")
                    {
                        _returnvalue = System.Windows.Browser.HtmlPage.Document.QueryString[_s];
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return _returnvalue;
        }

        public void Init()
        {
            try
            {
                _fetchCategories();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void StartGetSavedCaseCategories(string id)
        {
            try
            {
                _id = id;
                _getSavesCaseCategories();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _getSavesCaseCategories()
        {
            try
            {
                if (!string.IsNullOrEmpty(_id))
                    _crmManager.Fetch<CaseCategory>(_getXmlSavesCaseCategories(_id), _getSavesCaseCategories_callback);
                else
                    _fetchCategories();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string _getXmlSavesCaseCategories(string id)
        {
            string _xml = "";

            _xml += "<fetch version='1.0' mapping='logical' >";
            _xml += "   <entity name='cgi_casecategory'>";
            _xml += "       <attribute name='cgi_casecategoryid' />";
            _xml += "       <attribute name='cgi_casecategoryname' />";
            _xml += "       <attribute name='cgi_category3id' />";
            _xml += "       <attribute name='cgi_category2id' />";
            _xml += "       <attribute name='cgi_category1id' />";
            _xml += "       <attribute name='cgi_caseid' />";
            _xml += "       <order attribute='cgi_casecategoryname' descending='false' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='cgi_caseid' operator='eq' value='" + id + "' />";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private void _getSavesCaseCategories_callback(ObservableCollection<CaseCategory> casecategory)
        {
            try
            {
                foreach (CaseCategory _category in casecategory)
                {
                    SelectedCategory _sel = _createSelection(_category);
                    _selectionList.Add(_sel);
                }

                _refreshDatabinding(true);
                _fetchCategories();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _fetchCategories()
        {
            try
            {
                _crmManager.Fetch<CategoryDetail>(_fetchXmlCategories(), _fetchCategories_callBack);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string _fetchXmlCategories()
        {
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' count='-1' >";
            _xml += "    <entity name='cgi_categorydetail' >";
            _xml += "        <attribute name='cgi_categorydetailname' />";
            _xml += "        <attribute name='cgi_sortorder' />";
            _xml += "        <attribute name='cgi_parentid' />";
            _xml += "        <attribute name='cgi_color' />";
            _xml += "        <attribute name='cgi_categorydetailid' />";
            _xml += "        <filter type='and' >";
            _xml += "            <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "        </filter>";
            _xml += "    </entity>";
            _xml += "</fetch>";

            return _xml;
        }

        private void _fetchCategories_callBack(ObservableCollection<CategoryDetail> categorydetails)
        {
            try
            {
                //save orginal list for laster use.
                _originalCategoryDetails = categorydetails.ToList();
                _categoryitems = new List<ItemRow>();
                //get category1. parent attribute is null.
                List<CategoryDetail> _category1 = categorydetails.Where(x => x.Parent == null).OrderBy(x => x.Sortorder).ThenBy(x => x.Name).ToList();
                List<ItemRow> _items = new List<ItemRow>();

                //Add level 1 and 2 to the list.
                foreach (CategoryDetail _c1 in _category1)
                {
                    List<CategoryDetail> _category2 = categorydetails.Where(x => x.Parent == _c1.CategoryDetailId).OrderBy(x => x.Sortorder).ToList();
                    
                    foreach (CategoryDetail _c2 in _category2)
                    {
                        ItemRow _rowitem = new ItemRow();
                        _rowitem.Text1 = _c1.Name;
                        _rowitem.Text2 = _c2.Name;
                        _rowitem.Text3 = "";
                        _rowitem.Text1ID = _c1.CategoryDetailId;
                        _rowitem.Text2ID = _c2.CategoryDetailId;
                        _rowitem.Text3ID = null;
                        _rowitem.BackgroundBrush1 = _setBackround(_c1.Color);
                        _rowitem.BackgroundBrush2 = _setBackround(_c1.Color);
                        _rowitem.BackgroundBrush3 = _setBackround(_c1.Color);
                        _rowitem.Sortorder1 = (_c1.Sortorder != null) ? Convert.ToInt32(_c1.Sortorder) : 0;
                        _rowitem.Sortorder2 = (_c2.Sortorder != null) ? Convert.ToInt32(_c2.Sortorder) : 0;
                        _rowitem.Sortorder3 = 0;
                        _categoryitems.Add(_rowitem);
                        _items.Add(_rowitem);
                    }
                }

                //Add level 3 to the list.
                foreach (ItemRow _row in _items)
                {
                    List<CategoryDetail> _category3 = categorydetails.Where(x => x.Parent == _row.Text2ID).OrderBy(x => x.Sortorder).ToList();

                    foreach (CategoryDetail _c3 in _category3)
                    {
                        ItemRow _rows = _categoryitems.FirstOrDefault(x => x.Text2ID == _c3.Parent && x.Text3ID == null);
                        if (_rows != null)
                        {
                            _rows.Text3 = _c3.Name;
                            _rows.Text3ID = _c3.CategoryDetailId;
                            _rows.Sortorder3 = (_c3.Sortorder != null) ? Convert.ToInt32(_c3.Sortorder) : 0;
                        }
                        else if (_rows == null)
                        {
                            ItemRow _r = new ItemRow();
                            _r.Text1 = _row.Text1;
                            _r.Text1ID = _row.Text1ID;
                            _r.Text2 = _row.Text2;
                            _r.Text2ID = _row.Text2ID;
                            _r.Text3 = _c3.Name;
                            _r.Text3ID = _c3.CategoryDetailId;
                            _r.BackgroundBrush1 = _setBackround(_c3.Color);
                            _r.BackgroundBrush2 = _setBackround(_c3.Color);
                            _r.BackgroundBrush3 = _setBackround(_c3.Color);
                            _r.Sortorder1 = _row.Sortorder1;
                            _r.Sortorder2 = _row.Sortorder2;
                            _r.Sortorder3 = (_c3.Sortorder != null) ? Convert.ToInt32(_c3.Sortorder) : 0;
                            _categoryitems.Add(_r);
                        }
                    }
                }

                _fillTree(_categoryitems);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _fillTree(List<ItemRow> list)
        {
            try
            {
                _sorted1 = _sortList(list);
                _setTree();
                _sorted2 = _sortList(_newItems);

                _theDispather.BeginInvoke(() => { _grdGrid.ItemsSource = _sorted2; });
                _theDispather.BeginInvoke(() => { SetTreeState(); });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _setTree()
        {
            try
            {
                _newItems = new List<ItemRow>();
                string _c1 = "";
                string _c2 = "";
                string _c3 = "";
                int x = 1;

                foreach (ItemRow _row in _sorted1)
                {
                    
                    if (_row.Text2 != "" && _row.Text3 == "")
                    {
                        _row.Command1.IsEnabled = false;
                        _row.Command2.IsEnabled = true;
                        _row.Command3.IsEnabled = false;
                    }

                    if (x == 1 || _c1 != _row.Text1)
                    {
                        _setCategory1(_row, x);
                        x = 99;
                        _c1 = _row.Text1;
                        _c2 = _row.Text2;
                        _c3 = _row.Text3;
                    }

                    if (_c2 != _row.Text2)
                    {
                        _setCategory2(_row);
                        _c1 = _row.Text1;
                        _c2 = _row.Text2;
                        _c3 = _row.Text3;
                    }

                    if (_row.Text3 != "")
                    {
                        _row.Command1.IsEnabled = false;
                        _row.Command2.IsEnabled = false;
                        _row.Command3.IsEnabled = true;
                        _setCategory3(_row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _setCategory1(ItemRow item, int row)
        {
            try
            {
                Thickness _b = new Thickness();
                brushCurrent = item.BackgroundBrush1;

                if (row == 1)
                    _b = new Thickness(0, 0, 0, 0);
                else
                    _b = new Thickness(0, 1, 0, 0);

                ItemRow newitem = new ItemRow
                {
                    Text1 = item.Text1,
                    Text1ID = item.Text1ID,
                    Text2 = item.Text2,
                    Text2ID = item.Text2ID,
                    Text3 = item.Text3,
                    Text3ID = item.Text3ID,
                    BorderThickness1 = _b,
                    BorderThickness2 = _b,
                    BorderThickness3 = _b,
                    Visibility1 = System.Windows.Visibility.Visible,
                    Visibility2 = System.Windows.Visibility.Collapsed,
                    Visibility3 = System.Windows.Visibility.Collapsed,
                    BackgroundBrush1 = brushCurrent,                        //item.BackgroundBrush1,
                    BackgroundBrush2 = brushCurrent,                        //item.BackgroundBrush1,
                    BackgroundBrush3 = brushCurrent,                        //item.BackgroundBrush1,
                    CaseId = _id,
                    ParentViewModel = this
                };
                newitem.Command1.IsEnabled = false;
                newitem.Command2.IsEnabled = false;
                newitem.Command3.IsEnabled = false;
                _newItems.Add(newitem);

                newitem = new ItemRow
                {
                    Text1 = item.Text1,
                    Text2 = item.Text2,
                    Text3 = item.Text3,
                    Text1ID = item.Text1ID,
                    Text2ID = item.Text2ID,
                    Text3ID = item.Text3ID,
                    BorderThickness1 = new Thickness(0, 0, 0, 0),
                    BorderThickness2 = new Thickness(0, 0, 0, 0),
                    BorderThickness3 = new Thickness(0, 0, 0, 0),
                    Visibility1 = System.Windows.Visibility.Collapsed,
                    Visibility2 = System.Windows.Visibility.Visible,
                    Visibility3 = System.Windows.Visibility.Collapsed,
                    BackgroundBrush1 = brushCurrent,                        //item.BackgroundBrush1,
                    BackgroundBrush2 = brushCurrent,                        //item.BackgroundBrush1,
                    BackgroundBrush3 = brushCurrent,                        //item.BackgroundBrush1,
                    CaseId = _id,
                    ParentViewModel = this,
                    TextDec1 = null,
                    TextDec2 = (item.Text3 == "") ? TextDecorations.Underline : null,
                    TextDec3 = null
                };
                newitem.Command1.IsEnabled = item.Command1.IsEnabled;
                newitem.Command2.IsEnabled = item.Command2.IsEnabled;
                newitem.Command3.IsEnabled = item.Command3.IsEnabled;
                _newItems.Add(newitem);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _setCategory2(ItemRow item)
        {
            try
            {
                ItemRow newitem = new ItemRow
                {
                    Text1 = item.Text1,
                    Text2 = item.Text2,
                    Text3 = item.Text3,
                    Text1ID = item.Text1ID,
                    Text2ID = item.Text2ID,
                    Text3ID = item.Text3ID,
                    BorderThickness1 = new Thickness(0, 0, 0, 0),
                    BorderThickness2 = new Thickness(0, 0, 0, 0),
                    BorderThickness3 = new Thickness(0, 0, 0, 0),
                    Visibility1 = System.Windows.Visibility.Collapsed,
                    Visibility2 = System.Windows.Visibility.Visible,
                    Visibility3 = System.Windows.Visibility.Collapsed,
                    BackgroundBrush1 = brushCurrent,                            //item.BackgroundBrush1,
                    BackgroundBrush2 = brushCurrent,                            //item.BackgroundBrush1,
                    BackgroundBrush3 = brushCurrent,                            //item.BackgroundBrush1,
                    CaseId = _id,
                    ParentViewModel = this,
                    TextDec1 = null,
                    TextDec2 = (item.Text3 == "") ? TextDecorations.Underline : null,
                    TextDec3 = null
                };
                newitem.Command1.IsEnabled = item.Command1.IsEnabled;
                newitem.Command2.IsEnabled = item.Command2.IsEnabled;
                newitem.Command3.IsEnabled = item.Command3.IsEnabled;
                _newItems.Add(newitem);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _setCategory3(ItemRow item)
        {
            try
            {
                ItemRow newitem = new ItemRow
                {
                    Text1 = item.Text1,
                    Text2 = item.Text2,
                    Text3 = item.Text3,
                    Text1ID = item.Text1ID,
                    Text2ID = item.Text2ID,
                    Text3ID = item.Text3ID,
                    BorderThickness1 = new Thickness(0, 0, 0, 0),
                    BorderThickness2 = new Thickness(0, 0, 0, 0),
                    BorderThickness3 = new Thickness(0, 0, 0, 0),
                    Visibility1 = System.Windows.Visibility.Collapsed,
                    Visibility2 = System.Windows.Visibility.Collapsed,
                    Visibility3 = System.Windows.Visibility.Visible,
                    BackgroundBrush1 = brushCurrent,                            //item.BackgroundBrush1,
                    BackgroundBrush2 = brushCurrent,                            //item.BackgroundBrush1,
                    BackgroundBrush3 = brushCurrent,                            //item.BackgroundBrush1,
                    CaseId = _id,
                    ParentViewModel = this,
                    TextDec1 = null,
                    TextDec2 = null,
                    TextDec3 = TextDecorations.Underline
                };
                newitem.Command1.IsEnabled = item.Command1.IsEnabled;
                newitem.Command2.IsEnabled = item.Command2.IsEnabled;
                newitem.Command3.IsEnabled = item.Command3.IsEnabled;
                _newItems.Add(newitem);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private Brush _setBackround(int crmColor)
        {
            Brush _returnBrush = null;

            if (crmColor == 285050000)
                _returnBrush = brushA;
            else if (crmColor == 285050001)
                _returnBrush = brushB;
            else if (crmColor == 285050002)
                _returnBrush = brushC;
            else if (crmColor == 285050003)
                _returnBrush = brushD;
            else
                _returnBrush = brushDefault;

            return _returnBrush;
        }

        private List<ItemRow> _sortList(List<ItemRow> list)
        {
            try
            {
                return list.OrderBy(x => x.Sortorder1).ThenBy(x => x.Sortorder2).ThenBy(x => x.Sortorder3).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteSelection(SelectedCategory selectedcategory)
        {
            try
            {
                if (_caseStateCode != "0")
                    return;

                if (selectedcategory.ParentCaseCategory != null)
                {
                    CaseCategory _cc = selectedcategory.ParentCaseCategory as CaseCategory;
                    _crmManager.Delete(_cc);
                    _selectionList.Remove(selectedcategory);

                    int _count;
                    if (_selectionList.Count() <= 0)
                        _count = 0;
                    else
                        _count = _selectionList.Count();

                    _setTreeviewClick(_count);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            _refreshDatabinding(false);         
        }

        private void _setTreeviewClick(int count)
        {
            try
            {
                if (_debug != "yes")
                    System.Windows.Browser.HtmlPage.Window.Invoke("setTreeviewClick", count.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SaveToCRM(ItemRow itemrow)
        {
            try
            {
                bool _updated = false;

                if (_id == "" || _id == null || string.IsNullOrWhiteSpace(_id))
                    return;

                CaseCategory _cc = _createCasecategoryEntity(itemrow);
                //check if category1 is found
                SelectedCategory _sel = _selectionList.FirstOrDefault(x =>
                    x.Category1 == itemrow.Text1ID &&
                    x.Category2 == null &&
                    x.Category3 == null);

                if (_sel != null)
                {
                    _updated = true;
                    _sel.Caption = itemrow.Name;
                    _sel.Category2 = itemrow.Text2ID;
                    _sel.Category3 = itemrow.Text3ID;
                    _cc.CaseCategoryId = _sel.CaseCategoryId;
                    _sel.ParentCaseCategory = _cc;
                }

                if (_sel == null)
                {
                    //check if category1 and category2 is found
                    _sel = _selectionList.FirstOrDefault(x =>
                        x.Category1 == itemrow.Text1ID &&
                        x.Category2 == itemrow.Text2ID &&
                        x.Category3 == null);
                    if (_sel != null)
                    {
                        _updated = true;
                        _sel.Caption = itemrow.Name;
                        _sel.Category3 = itemrow.Text3ID;
                        _cc.CaseCategoryId = _sel.CaseCategoryId;
                        _sel.ParentCaseCategory = _cc;
                    }
                }

                if (_updated == false)
                {
                    _createCaseCategory(_cc);
                }
                else if (_updated == true)
                {
                    _updateCaseCategory(_cc);
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddToSelection(ItemRow itemrow)
        {
            try
            {
                if (_caseStateCode != "0")
                    return;

                SelectedCategory _selcat = _createSelection(itemrow);
                _selcat.DeleteCommand.IsEnabled = true;

                if (!_selectionList.Contains(_selcat))
                {
                    _selectionList.Add(_selcat);
                    SaveToCRM(itemrow);

                    int _count;
                    if (_selectionList.Count() <= 0)
                        _count = 0;
                    else
                        _count = _selectionList.Count();

                    _setTreeviewClick(_count);
                }

                _refreshDatabinding(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _createCaseCategory(CaseCategory casecategory)
        {
            try
            {
                _crmManager.Create(casecategory, _casecategory_create_callback);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _updateCaseCategory(CaseCategory casecategory)
        {
            try
            {
                _crmManager.Update(casecategory, _catecategory_update_callback);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _casecategory_create_callback(CaseCategory casecategory)
        {
            try
            {
                Guid? _c1 = null;
                Guid? _c2 = null;
                Guid? _c3 = null;

                if (casecategory.Category1Id != null)
                    _c1 = casecategory.Category1Id.Id;
                if (casecategory.Category2Id != null)
                    _c2 = casecategory.Category2Id.Id;
                if (casecategory.Category3Id != null)
                    _c3 = casecategory.Category3Id.Id;

                SelectedCategory _sel = _selectionList.FirstOrDefault(x =>
                    x.Category1 == _c1 &&
                    x.Category2 == _c2 &&
                    x.Category3 == _c3);
                if (_sel != null)
                {
                    _sel.CaseCategoryId = casecategory.CaseCategoryId;
                    _sel.ParentCaseCategory = casecategory;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _catecategory_update_callback(CaseCategory casecategory)
        { 
        
        }
        
        public bool CheckIfSelectionExists(ItemRow itemrow)
        {
            bool _returnvalue = false;
            SelectedCategory _sc = _selectionList.FirstOrDefault(x => x.Caption == itemrow.Name);
            if (_sc != null)
                _returnvalue = true;

            return _returnvalue;
        }

        private void _addCaseCategoryToHTML(bool startup)
        {
            try
            {
                List<SelectedCategory> _sort = _selectionList.OrderBy(x => x.Caption).ToList();
                _lstListBox.ItemsSource = null;
                _lstListBox.ItemsSource = _sort;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void _refreshDatabinding(bool startup)
        {
            _theDispather.BeginInvoke(() =>
            {
                try
                {
                    _addCaseCategoryToHTML(startup);    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }

        private SelectedCategory _createSelection(ItemRow itemrow)
        {
            SelectedCategory _selcat = null;

            try
            {
                Guid? _c1 = null;
                Guid? _c2 = null;
                Guid? _c3 = null;

                if (itemrow.Text1ID != null)
                    _c1 = itemrow.Text1ID;
                if (itemrow.Text2ID != null)
                    _c2 = itemrow.Text2ID;
                if (itemrow.Text3ID != null)
                    _c3 = itemrow.Text3ID;

                CaseCategory _cc = _createCasecategoryEntity(itemrow);
                
                _selcat = new SelectedCategory
                {
                    CaseCategoryId = _cc.CaseCategoryId,
                    Caption = _cc.Name,
                    Category1 = _c1,
                    Category2 = _c2,
                    Category3 = _c3,
                    ParentViewModel = this,
                    ParentCaseCategory = _cc,
                    ItemRow = itemrow
                };
                _selcat.DeleteCommand.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _selcat;
        }

        private SelectedCategory _createSelection(CaseCategory casecategory)
        {
            SelectedCategory _selcat = null;

            try
            {
                Guid? _c1 = null;
                Guid? _c2 = null;
                Guid? _c3 = null;

                if (casecategory.Category1Id != null)
                    _c1 = casecategory.Category1Id.Id;
                if (casecategory.Category2Id != null)
                    _c2 = casecategory.Category2Id.Id;
                if (casecategory.Category3Id != null)
                    _c3 = casecategory.Category3Id.Id;

                ItemRow _ir = new ItemRow
                {
                    Text1 = casecategory.Category1Id.Name,
                    Text2 = casecategory.Category2Id.Name,
                    Text3 = casecategory.Category3Id.Name,
                    Text1ID = casecategory.Category1Id.Id,
                    Text2ID = casecategory.Category2Id.Id,
                    Text3ID = casecategory.Category3Id.Id,
                    BorderThickness1 = new Thickness(0, 0, 0, 0),
                    BorderThickness2 = new Thickness(0, 0, 0, 0),
                    BorderThickness3 = new Thickness(0, 0, 0, 0),
                    Visibility1 = System.Windows.Visibility.Collapsed,
                    Visibility2 = System.Windows.Visibility.Collapsed,
                    Visibility3 = System.Windows.Visibility.Visible,
                    BackgroundBrush1 = brushCurrent,                            //item.BackgroundBrush1,
                    BackgroundBrush2 = brushCurrent,                            //item.BackgroundBrush1,
                    BackgroundBrush3 = brushCurrent,                            //item.BackgroundBrush1,
                    CaseId = _id,
                    ParentViewModel = this,
                    TextDec1 = null,
                    TextDec2 = null,
                    TextDec3 = TextDecorations.Underline
                };


                _selcat = new SelectedCategory
                {
                    CaseCategoryId = casecategory.CaseCategoryId,
                    Caption = casecategory.Name,
                    Category1 = _c1,
                    Category2 = _c2,
                    Category3 = _c3,
                    ParentViewModel = this,
                    ParentCaseCategory = casecategory,
                    ItemRow = _ir
                };
                _selcat.DeleteCommand.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _selcat;
        }

        private CaseCategory _createCasecategoryEntity(ItemRow itemrow)
        {
            CaseCategory _cc = null;

            try
            {
                _cc = new CaseCategory();
                _cc.Category1Id = _createEntRef(itemrow.Text1ID.ToString(), itemrow.Text1, "cgi_categorydetail");
                _cc.Category2Id = _createEntRef(itemrow.Text2ID.ToString(), itemrow.Text2, "cgi_categorydetail");
                _cc.Category3Id = _createEntRef(itemrow.Text3ID.ToString(), itemrow.Text3, "cgi_categorydetail");
                _cc.CaseId = _createEntRef(itemrow.CaseId, "", "incident");
                _cc.Name = itemrow.Name;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _cc;
        }

        private EntityReference _createEntRef(string id, string name, string logicalname)
        {
            EntityReference _entref = null;

            if (id != null && id != "" && !string.IsNullOrEmpty(id))
            {
                _entref = new EntityReference
                {
                    Id = new Guid(id),
                    LogicalName = logicalname,
                    Name = name
                };
            }

            return _entref;
        }

        private void SetTreeState()
        {
            /*
                0 = Active
                1 = Resolved
                2 = Canceled
            */

            try
            {
                if (_debug != "yes")
                {
                    string _statecode = System.Windows.Browser.HtmlPage.Window.Invoke("getCaseStatus").ToString();
                    _caseStateCode = _statecode;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
