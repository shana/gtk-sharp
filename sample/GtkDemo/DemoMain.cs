//
// DemoMain.cs, port of main.c from gtk-demo
//
// Author: Daniel Kornhauser <dkor@alum.mit.edu>
//
// Copyright (C) 2003, Ximian Inc.

using System;
using System.IO;
using System.Collections;

using Gdk;
using Gtk;
using GtkSharp;
using Pango;

namespace GtkDemo 
{
	public class DemoMain
	{
		private Gtk.Window window;
		private TextBuffer infoBuffer = new TextBuffer(null);
		private TextBuffer sourceBuffer = new TextBuffer(null);
		private TreeView treeView; 
		private TreeStore store;
		public static void Main (string[] args)
		{
			Application.Init ();
			new DemoMain ();
			Application.Run ();
		}
	
		public DemoMain ()
		{  
			SetupDefaultIcon ();
		    window = new Gtk.Window ("Gtk# Code Demos");
		    window.SetDefaultSize (600,400);
			window.DeleteEvent += new DeleteEventHandler (WindowDelete);

			HBox hbox = new HBox (false, 0);
			window.Add (hbox);
			
			TreeView tree = CreateTree ();
			tree.AppendColumn ("Widget (double click for demo)", new CellRendererText (), "text", 0);
			tree.Model = FillTree ();
			tree.Selection.Changed += new EventHandler (OnTreeChanged);
			tree.RowActivated += new RowActivatedHandler (OnRowActivated);
			hbox.PackStart (tree, false, false, 0);
			
			Notebook notebook = new Notebook ();
			hbox.PackStart (notebook, true, true, 0);

			notebook.AppendPage (CreateText (infoBuffer, false), Label.NewWithMnemonic ("_Info"));
			notebook.AppendPage (CreateText (sourceBuffer, true), Label.NewWithMnemonic ("_Source"));

			window.ShowAll ();
		}

		private void LoadFile(string filename)
		{

			Stream file = File.OpenRead(filename);
			StreamReader sr = new StreamReader(file);
			string s = sr.ReadToEnd();
			sr.Close();
			file.Close();

			infoBuffer.Text = filename;
			sourceBuffer.Text = s;

			Fontify();
		}

		private void Fontify()
		{
		}

		// TODO: Display system error
		private void SetupDefaultIcon ()
		{
			Gdk.Pixbuf pixbuf = new Gdk.Pixbuf ("images/gtk-logo-rgb.gif");
		
			if (pixbuf != null)
			{
				// The gtk-logo-rgb icon has a white background, make it transparent
				Pixbuf transparent  = pixbuf.AddAlpha (true, 0xff, 0xff, 0xff);
			
				GLib.List list = new GLib.List (IntPtr.Zero, typeof (Gtk.Widget));
				list.Append (transparent.Handle);
			
		    	Gtk.Window.DefaultIconList = list;
			}
		}

		// BUG: I have to click twice close in order for the dialog to disappear
		private void ResponseCallback (object obj, ResponseArgs args)
		{
			Dialog dialog = (Dialog) obj ;
			dialog.Destroy ();
		}

		private ScrolledWindow CreateText (TextBuffer buffer, bool IsSource)
		{
			ScrolledWindow scrolledWindow = new ScrolledWindow ();
			scrolledWindow.SetPolicy (PolicyType.Automatic, PolicyType.Automatic);
			scrolledWindow.ShadowType = ShadowType.In;
		
			TextView textView = new TextView ();
			
			textView.Buffer = buffer;
			// STUCK

			textView.Editable = false;
			textView.CursorVisible = false;

			scrolledWindow.Add (textView);

			if (IsSource)
			{
				FontDescription fontDescription = FontDescription.FromString ("Courier 12");	
				textView.ModifyFont (fontDescription);
				textView.WrapMode = Gtk.WrapMode.None;

			}
			else
			{
				// Make it a bit nicer for text
				textView.WrapMode = Gtk.WrapMode.Word;
				textView.PixelsAboveLines = 2;
				textView.PixelsBelowLines = 2;
			}
			
			return scrolledWindow;
        }
                                                            

        private TreeView CreateTree ()
		{
			treeView = new TreeView ();
			return treeView;
        }
        
        private TreeStore FillTree ()
        {
        	store = new TreeStore (typeof (string));
        	
        	store.AppendValues ("Application Window (5% complete)");
        	store.AppendValues ("Button Boxes");
        	store.AppendValues ("Change Display");
        	store.AppendValues ("Color Selector");
        	store.AppendValues ("Dialog and Message Boxes");
        	store.AppendValues ("Drawing Area");
        	store.AppendValues ("Images");
        	store.AppendValues ("Item Factory (5% complete)");
        	store.AppendValues ("Menus");
        	store.AppendValues ("Paned Widget");
        	store.AppendValues ("Pixbuf");
        	store.AppendValues ("Size Groups");
        	store.AppendValues ("Stock Item and Icon Browser (10% complete)");
        	store.AppendValues ("Text Widget (95% complete)");
        	TreeIter iter = store.AppendValues ("Tree View");
        	store.AppendValues (iter, "Editable Cells (buggy)");
        	store.AppendValues (iter, "List Store");
        	store.AppendValues (iter, "Tree Store (95% complete)");
        	
        	return store;
        }
        
        private void OnTreeChanged (object o, EventArgs args)
		{

		TreeIter iter;
		TreeModel model;

		if (treeView.Selection.GetSelected (out model, out iter))
		{
			TreePath path;
			path = store.GetPath (iter);

		switch (path.ToString()) {

        		case "0":
        			LoadFile ("DemoApplicationWindow.cs");
        			break;
        		case "1":
        			LoadFile ("DemoButtonBox.cs");
        			break;
        		case "2":
        			//
        			break;
        		case "3":
        			LoadFile ("DemoColorSelection.cs");
        			break;
        		case "4":
        			LoadFile ("DemoDialog.cs");
        			break;
        		case "5":
        			LoadFile ("DemoDrawingArea.cs");
        			break;
        		case "6":
        			LoadFile ("DemoImages.cs");
        			break;
        		case "7":
        			LoadFile ("DemoItemFactory.cs");
        			break;
        		case "8":
        			LoadFile ("DemoMenus.cs");
        			break;
        		case "9":
        			LoadFile ("DemoPanes.cs");
        			break;
        		case "10":
        			LoadFile ("DemoPixbuf.cs");
        			break;
        		case "11":
        			LoadFile ("DemoSizeGroup.cs");
        			break;
        		case "12":
        			LoadFile ("DemoStockBrowser.cs");
        			break;
        		case "13":
        			LoadFile ("DemoTextView.cs");
        			break;
        		case "14":
        			// do nothing
        			break;
        		case "14:0":
        			LoadFile ("DemoEditableCells.cs");
        			break;
        		case "14:1":
        			LoadFile ("DemoListStore.cs");
        			break;
        		case "14:2":
        			LoadFile ("DemoTreeStore.cs");
        			break;
        		default:
        			break;
		}

			}
        }

        private void OnRowActivated (object o, RowActivatedArgs args)
        {
        	switch (args.Path.ToString ()) {
        		case "0":
        			new DemoApplicationWindow ();
        			break;
        		case "1":
        			new DemoButtonBox ();
        			break;
        		case "2":
        			//
        			break;
        		case "3":
        			new DemoColorSelection ();
        			break;
        		case "4":
        			new DemoDialog ();
        			break;
        		case "5":
        			new DemoDrawingArea ();
        			break;
        		case "6":
        			new DemoImages ();
        			break;
        		case "7":
        			new DemoItemFactory ();
        			break;
        		case "8":
        			new DemoMenus ();
        			break;
        		case "9":
        			new DemoPanes ();
        			break;
        		case "10":
        			new DemoPixbuf ();
        			break;
        		case "11":
        			new DemoSizeGroup ();
        			break;
        		case "12":
        			new DemoStockBrowser ();
        			break;
        		case "13":
        			new DemoTextView ();
        			break;
        		case "14":
        			// do nothing
        			break;
        		case "14:0":
        			new DemoEditableCells ();
        			break;
        		case "14:1":
        			new DemoListStore ();
        			break;
        		case "14:2":
        			new DemoTreeStore ();
        			break;
        		default:
        			break;
			}
        }
                                                                                                                             
		private void WindowDelete (object o, DeleteEventArgs args)
		{
			Application.Quit ();
			args.RetVal = true;
		}
	}
}