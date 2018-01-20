new ScriptObject(PasteProvider)
{
};

function PasteProvider::readBook(%this, %tag)
{
  %this.web_last = openWeb(%this.getBookUrl(%tag));
}


function PasteProvider::getBookUrl(%this, %tag)
{
  %book_id = %this.getBookId(%tag);
  return %this.base_url @ %book_id;
}


function PasteProvider::getBookId(%this, %tag)
{
  %prefix_len = strlen(%this.prefix);
  return getSubStr(%tag, %prefix_len);
}


function PasteProvider::isCorrectTag(%this, %tag)
{
  return startsWith(%tag, %this.prefix);
}
