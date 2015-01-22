using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//using MonoTouch.UIKit;

namespace MagicMobile
{
   public class TableItem
	{
		public string Heading { get; set; }
		
		public string SubHeading { get; set; }
		
		public string ImageName { get; set; }

        public int ImageResourceId { get; set; }
		
     //  public ImageResourceId  d {get; set; }
       /*
		public UITableViewCellStyle CellStyle
		{
			get { return cellStyle; }
			set { cellStyle = value; }
		}
		protected UITableViewCellStyle cellStyle = UITableViewCellStyle.Default;
		
		public UITableViewCellAccessory CellAccessory
		{
			get { return cellAccessory; }
			set { cellAccessory = value; }
		}
		protected UITableViewCellAccessory cellAccessory = UITableViewCellAccessory.None;*/

        

		public TableItem () { }
		
		public TableItem (string heading)
		{ this.Heading = heading; }
	}
}
