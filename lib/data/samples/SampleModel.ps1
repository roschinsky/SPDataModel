Add-PSSnapin Microsoft.SharePoint.PowerShell

$rootUrl = "https://localhost/irgendwo/deinSharePointWeb"
$webRoot = Get-SPWeb $rootUrl
[System.Threading.Thread]::CurrentThread.CurrentUICulture = [System.Globalization.CultureInfo] 1031

# *** Create List Scope #1 ***
# ------------ Lists...
#  - - - - - - List "SalesOrder"
$lst1_01 = $webRoot.Lists.GetList($webRoot.Lists.Add("Auftrag", "Scope #1: Aufträge", $webRoot.ListTemplates["Benutzerdefinierte Liste"]), $false)
#  - - - - - - Fields "SalesOrder"
$field = $lst1_01.Fields[$lst1_01.Fields.Add("orderNumber", [Microsoft.SharePoint.SPFieldType]::Text, $true)]
$field.Title = "Auftragsnummer"
$field.Update()
$field = $lst1_01.Fields[$lst1_01.Fields.Add("extendedName", [Microsoft.SharePoint.SPFieldType]::Text, $false)]
$field.Title = "Bezeichnung"
$field.Update()
$fieldChoices = New-Object System.Collections.Specialized.StringCollection
$fieldChoices.Add("Offen")
$fieldChoices.Add("In Bearbeitung")
$fieldChoices.Add("Versendet")
$fieldChoices.Add("Storniert")
$fieldChoices.Add("Erfüllt")
$field = $lst1_01.Fields[$lst1_01.Fields.Add("statusCode", [Microsoft.SharePoint.SPFieldType]::Choice, $true, $false, $fieldChoices)]
$field.Title = "Auftragstyp"
$field.Update()
$fieldChoices = New-Object System.Collections.Specialized.StringCollection
$fieldChoices.Add("Shop")
$fieldChoices.Add("Mail")
$fieldChoices.Add("Telefon")
$fieldChoices.Add("Sonstiges")
$field = $lst1_01.Fields[$lst1_01.Fields.Add("sourceOfOrder", [Microsoft.SharePoint.SPFieldType]::Choice, $true, $false, $fieldChoices)]
$field.Title = "Quelle"
$field.Update()
$field = $lst1_01.Fields[$lst1_01.Fields.Add("orderDescription", [Microsoft.SharePoint.SPFieldType]::Note, $false)]
$field.Title = "Beschreibung"
$field.Update()
#  - - - - - - List "SalesOrderItem"
$lst1_02 = $webRoot.Lists.GetList($webRoot.Lists.Add("Auftragsposition", "Scope #1: Auftragspositionen", $webRoot.ListTemplates["Benutzerdefinierte Liste"]), $false)
#  - - - - - - Fields "SalesOrderItem"
$field = $lst1_02.Fields[$lst1_02.Fields.Add("itemAmount", [Microsoft.SharePoint.SPFieldType]::Number, $true)]
$field.Title = "Menge"
$field.Update()
$field = $lst1_02.Fields[$lst1_02.Fields.Add("itemSinglePrice", [Microsoft.SharePoint.SPFieldType]::Currency, $false)]
$field.Title = "Einzelpreis"
$field.Update()
$field = $lst1_02.Fields[$lst1_02.Fields.Add("itemTotalPrice", [Microsoft.SharePoint.SPFieldType]::Currency, $false)]
$field.Title = "Einzelpreis"
$field.Update()
$field = $lst1_02.Fields[$lst1_02.Fields.Add("isActive", [Microsoft.SharePoint.SPFieldType]::Boolean, $true)]
$field.Title = "Aktiv"
$field.Update()
#  - - - - - - List "Product"
$lst1_03 = $webRoot.Lists.GetList($webRoot.Lists.Add("Produkt", "Scope #1: Produkt", $webRoot.ListTemplates["Benutzerdefinierte Liste"]), $false)
#  - - - - - - Fields "Product"
$field = $lst1_03.Fields[$lst1_03.Fields.Add("productNumber", [Microsoft.SharePoint.SPFieldType]::Text, $false)]
$field.Title = "Produktnummer"
$field.Update()
$field = $lst1_03.Fields[$lst1_03.Fields.Add("itemPrice", [Microsoft.SharePoint.SPFieldType]::Currency, $false)]
$field.Title = "Einzelpreis"
$field.Update()
$fieldChoices = New-Object System.Collections.Specialized.StringCollection
$fieldChoices.Add("Stück")
$fieldChoices.Add("Packung")
$fieldChoices.Add("Palette")
$field = $lst1_03.Fields[$lst1_03.Fields.Add("typeOfAmount", [Microsoft.SharePoint.SPFieldType]::Choice, $true, $false, $fieldChoices)]
$field.Title = "Einheit"
$field.Update()
$field = $lst1_03.Fields[$lst1_03.Fields.Add("isActive", [Microsoft.SharePoint.SPFieldType]::Boolean, $true)]
$field.Title = "Aktiv"
$field.Update()
# ------------ Relations...
$lkup = $lst1_02.Fields[$lst1_02.Fields.AddLookup("lkupSalesOrder", $lst1_01.ID, $true)]
$lkup.LookupField = "Title"
$lkup.Title = "Auftrag"
$lkup.Update()
$lkup = $lst1_02.Fields[$lst1_02.Fields.AddLookup("lkupProduct", $lst1_03.ID, $true)]
$lkup.LookupField = "Title"
$lkup.Title = "Produkt"
$lkup.Update()
# ------------ Sort columns...
#  - - - - - - List "SalesOrder"
$reorderCt = $lst1_01.ContentTypes[0]
$newOrder = @("Title", "orderNumber", "extendedName", "statusCode", "sourceOfOrder", "orderDescription")
$reorderCt.FieldLinks.Reorder($newOrder)
$reorderCt.Update()
#  - - - - - - List "SalesOrderItem"
$reorderCt = $lst1_02.ContentTypes[0]
$newOrder = @("Title", "lkupSalesOrder", "lkupProduct", "itemAmount", "itemSinglePrice", "itemTotalPrice", "isActive")
$reorderCt.FieldLinks.Reorder($newOrder)
$reorderCt.Update()
#  - - - - - - List "Product"
$reorderCt = $lst1_03.ContentTypes[0]
$newOrder = @("Title", "productNumber", "itemPrice", "typeOfAmount", "isActive")
$reorderCt.FieldLinks.Reorder($newOrder)
$reorderCt.Update()

# *** Create List Scope #2***
# ------------ Lists...
#  - - - - - - List "2.1"
$lst2_01 = $webRoot.Lists.GetList($webRoot.Lists.Add("Zwei-Eins", "Scope #2: Zwei-Eins", $webRoot.ListTemplates["Benutzerdefinierte Liste"]), $false)
#  - - - - - - Fields "2.1"
$field = $lst2_01.Fields[$lst2_01.Fields.Add("isActive", [Microsoft.SharePoint.SPFieldType]::Boolean, $true)]
$field.Title = "Aktiv"
$field.Update()
$field = $lst2_01.Fields[$lst2_01.Fields.Add("itemDescription", [Microsoft.SharePoint.SPFieldType]::Note, $false)]
$field.Title = "Beschreibung"
$field.Update()
$field = $lst2_01.Fields[$lst2_01.Fields.Add("userGroups", [Microsoft.SharePoint.SPFieldType]::User, $true)]
$field.Title = "Nutzer/Gruppen"
$field.AllowMultipleValues = $true
$field.SelectionMode = [Microsoft.SharePoint.SPFieldUserSelectionMode]::PeopleAndGroups
$field.Update()
#  - - - - - - List "2.2"
$lst2_02 = $webRoot.Lists.GetList($webRoot.Lists.Add("Zwei-Zwei", "Scope #2: Zwei-Zwei", $webRoot.ListTemplates["Benutzerdefinierte Liste"]), $false)
#  - - - - - - Fields "2.2"
$field = $lst2_02.Fields[$lst2_02.Fields.Add("isActive", [Microsoft.SharePoint.SPFieldType]::Boolean, $true)]
$field.Title = "Aktiv"
$field.Update()
$field = $lst2_02.Fields[$lst2_02.Fields.Add("itemDescription", [Microsoft.SharePoint.SPFieldType]::Note, $false)]
$field.Title = "Beschreibung"
$field.Update()
$field = $lst2_02.Fields[$lst2_02.Fields.Add("contact", [Microsoft.SharePoint.SPFieldType]::User, $true)]
$field.Title = "Ansprechpartner"
$field.AllowMultipleValues = $false
$field.SelectionMode = [Microsoft.SharePoint.SPFieldUserSelectionMode]::PeopleOnly
$field.Update()

# ------------ Relations..

# ------------ Sort columns...
#  - - - - - - List "2.1"
$reorderCt = $lst2_01.ContentTypes[0]
$newOrder = @("Title", "itemDescription", "userGroups", "isActive")
$reorderCt.FieldLinks.Reorder($newOrder)
$reorderCt.Update()
#  - - - - - - List "2.2"
$reorderCt = $lst2_02.ContentTypes[0]
$newOrder = @("Title", "itemDescription", "contact", "isActive")
$reorderCt.FieldLinks.Reorder($newOrder)
$reorderCt.Update()
