new ScriptObject(book_mod)
{
  class = BookMod;
  highlighted_item_id = -1;
  pastebin_web_prefix = "http://pastebin.com/raw/";
  mod_folder = "art/mod/booksforthepeople/";
  book_bitmap = "art/2d/items/pages.png";
};



// Book reading

function BookMod::tryReadBook(%this, %keystate)
{
  if (%keystate == 0) return;
  %obj = %this.highlighted_item_id;
  if (%this.highlighted_item_id > 0)
  {
    %addr = %this.getAddr(%this.highlighted_item_id);
    if (%addr !$= "")
      %this.readBook(%addr);
  }
}


function BookMod::readBook(%this, %addr)
{
  %url = %this.pastebin_web_prefix @ %addr;
  openWeb(%url);
}

// Renaming books

function BookMod::setHighlightedBookName(%this)
{
  %this.setBookName(%this.highlighted_item_id);
}


function BookMod::setBookNamesInAllContainers(%this)
{
  echo("Setting book names");
  %i_max = PlayGui.getCount();
  for (%i=0;%i<%i_max;%i++)
  {
    %obj = PlayGui.getObject(%i);
    if (%obj.isMemberOfClass("GuiInventoryContainer"))
    {
      %this.setBookNamesInContainer(%obj);
    }
  }
  %this.schedule(1000, "setBookNamesInAllContainers");
}



function BookMod::setBookNamesInContainer(%this, %container)
{
  echo("in container");
  %i_max = %container.getCount();
  for (%i=0; %i<%i_max; %i++)
  {
    %obj = %container.getObject(%i);
    if (%obj.isMemberOfClass("GuiInventoryItem"))
    {
      echo("found item");
      %this.setBookName(%obj);
    }
  }
}


function BookMod::setBookName(%this, %item_id)
{
  %tag = %this.getAddrTag(%item_id);
  echo("TAG IS", %tag);
  if (%this.isCorrectTag(%tag))
  {
    %item_id.stored_tag = %tag;
    if (!%this.tryRenameBookFromFile(%item_id)) {
      %downloader = new ScriptObject() {
        class = BookModDownloader;
      };
      %downloader.start(%item_id);
    }
  }
}

function BookMod::getBookFilename(%this, %item_id)
{
   return %this.mod_folder @ %this.getAddrTag(%item_id);
}

function BookMod::getBookUrl(%this, %item_id)
{
  return %this.pastebin_web_prefix @ %this.getAddr(%item_id);
}

function BookMod::tryRenameBookFromFile(%this, %item_id)
{
  %file_read = new FileObject();
  %result = %file_read.OpenForRead(%this.getBookFilename(%item_id));

  if (%result) {
    %line = %file_read.readline();
    %item_id.setText(%line);
    %item_id.setBitmap(%this.book_bitmap);
    echo(%item_id.bitmap);
    return 1;
  }
  return 0;
}


// Utility

function BookMod::setHighlightedItemId(%this, %item_id)
{
  %this.highlighted_item_id = %item_id;
}


function BookMod::getAddr(%this, %item_id)
{
  %addr_tag = %this.getAddrTag(%item_id);
  if (%this.isCorrectTag(%addr_tag))
  {
    echo("Correct tag found");
    return getSubStr(%addr_tag, 2);
  }
  return "";
}


function BookMod::getAddrTag(%this, %item_id)
{
  if (%item_id.stored_tag $= ""){
    echo("No tag stored - ", %item_id.getText());
    return %item_id.getText();
  }
  else {
    echo("ST:", %item_id.stored_tag);
    return %item_id.stored_tag;
  }
}


function BookMod::isCorrectTag(%this, %tag)
{
  return (startsWith(%tag, "_p"));
}


// HTTP procedures
function BookModDownloader::start(%this, %item_id) {
  %this.item_id = %item_id;
  %tag = $book_mod_global.getAddrTag(%item_id);
  %addr = $book_mod_global.getAddr(%item_id);
  %this.download_helper = new SimDownloadHelper()
  {
    url = $book_mod_global.getBookUrl(%item_id);
    fileName = $book_mod_global.getBookFilename(%item_id);
  };
  %this.download_helper.start();
}

// Initialization

$book_mod_global = book_mod.getId();


function GuiInventoryItem::onMouseEnter(%this)
{
  Parent.onMouseEnter(%this);
  $book_mod_global.setHighlightedItemId(%this.getId());
}


GlobalActionMap.bindObj(keyboard, "ctrl p", "tryReadBook", $book_mod_global);
$book_mod_global.schedule(1000, "setBookNamesInAllContainers");
//usage: exec("art/mod/booksforthepeople/bookmod.cs");
