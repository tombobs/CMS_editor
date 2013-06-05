using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Editor2.Models;

namespace Editor2.Utils
{
    public class HtmlWriter
    {
        public static Dictionary<string, string> Mappings = new Dictionary<string, string>();
        //private static string FolderPath = "C:\\Users\\TomR\\";
        private static string ffs = @"<script>

                                    $('div.multi-element').hover(function(){
                                        jQuery(this).css({ ""border"": ""2px solid red"" });
    
                                    },function(){
                                        jQuery(this).css({ ""border"": ""none"" });
                                    });

                                    $('div.element').hover(function(){
                                        jQuery(this).css({ ""border"": ""2px solid red"" });
    
                                    },function(){
                                        jQuery(this).css({ ""border"": ""none"" });
                                    });

                                    $('div.element').click( function (e) {
	                                    var id = $(this).attr('id');
	                                    $('#edit-element').html(
		                                    $(this).html())
	                                    $('#edit-element').attr('contentEditable', true);
	
	                                    $('#myModal').toggle();
                                    })
                                    //$('#modal-title').html($(

                                    var children = $('div.multi-element').children();


                                    </script>";


        public static void SetUpMappings()
        {
            Mappings.Add("<b>", "<span class=\"bold\">");
            Mappings.Add("</b>", "</span>");
            Mappings.Add("<i>", "<span class=\"italic\">");
            Mappings.Add("</i>", "</span>");
            Mappings.Add("<u>", "<span class=\"underline\">");
            Mappings.Add("</u>", "</span>");
            //Mappings.Add
        }

        public static string GenerateHtml(DocModel doc)
        {
            string modal = @"<div id=""myModal"" class=""modal hide fade in"" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
                                <div class=""modal-header"">
                                    <button type=""button"" class=""close"" data-dismiss=""modal"" aria-hidden=""true"">×</button>
                                    <h3 id=""modal-title"">Editing - [PAGE_TITLE]</h3>
                                </div>
                                <div class=""modal-body"">
                                    <p id=""edit-element"">[CLICKED_ELEMENT]</p>
                                </div>
                                <div class=""modal-footer"">
                                    <button class=""btn"" data-dismiss=""modal"" aria-hidden=""true"">Close</button>
                                    <button id=""save-button"" class=""btn btn-primary"">Save changes</button>
                                </div>
                            </div>";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");

            sb.AppendLine("<html lang=\"en\">");
            
            sb.AppendLine("<head>");            
            sb.AppendLine("<link rel=\"stylesheet\" href=\"http://twitter.github.io/bootstrap/assets/css/docs.css\"></link>");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"http://twitter.github.io/bootstrap/assets/css/bootstrap-responsive.css\"></link>");
            sb.AppendLine("<link rel=\"stylesheet\" href=\"http://twitter.github.io/bootstrap/assets/css/bootstrap.css\"></link>");
            sb.AppendLine("<script src=\"http://twitter.github.io/bootstrap/assets/js/jquery.js\"></script>");
            sb.AppendLine("<script type=\"text/javascript\" src=\"Editor.js\"></script>");
            sb.AppendLine("<script src=\"http://twitter.github.io/bootstrap/assets/js/bootstrap-modal.js\"></script>");
            sb.AppendLine("<script src=\"http://twitter.github.io/bootstrap/assets/js/holder/holder.js\"></script>");
            
            sb.AppendLine("</head>");

            sb.AppendLine("<body>");
            sb.AppendLine("<div class=\"container\">");
            sb.AppendLine("<a onclick=\"LoadElement()\" href=\"#myModal\" role=\"button\" class=\"btn\" data-toggle=\"modal\">Launch demo modal</a>");
            sb.AppendLine(modal);
            sb.AppendLine("<h2 id=\"page-title\">" + doc.Title + "</h2>");

            sb.AppendLine("<div id=\"container\">");
            foreach (Element el in doc.Elements)
            {
                if (el.SubElements == null || el.SubElements.Count == 0)
                {
                    sb.AppendLine("<div class=\"element\" id=\"" + el.ElementId + "\">");
                    sb.AppendLine(HandleElement(el));
                    sb.AppendLine("</div>");
                }
                else
                {
                    sb.AppendLine("<div class=\"multi-element\" id=\"" + el.ElementId + "\">");
                    sb.AppendLine(HandleSubElements(el));
                    sb.AppendLine("</div>");
                }
            }
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            // FNAR : chrome won't load .js file so I#'m puting the script here.. sigh :(
            sb.Append(ffs);



            sb.AppendLine("</body>");

            sb.AppendLine("</html>");


            StreamWriter writer = new StreamWriter(Constants.HTMLFolderPath + doc.Title + "-" + doc.Type + ".html");
            writer.Write(sb.ToString());
            writer.Close();

            return sb.ToString();
        }

        public static string HandleElement(Element el)
        {
            //string id;
            //if (el.id != null && el.id != String.Empty)
            //{
            //    id = el.id;
            //}
            switch (el.Type)
            {
                case "Heading":
                    return "<h2>" + el.Content + "</h2>";
                case "SubHeading":
                    return "<h3>" + el.Content + "</h3>";
                case "Text":
                    return "<p>" + el.Content + "</p>";                
                case "Code":
                    return "<code>" + el.Content + "</code>";                
                case "OL":
                    return HandleOL(el);
                case "UL":
                    return HandleUL(el);     
                // default case will allow insertion of custom html
                default:                     
                    return el.Content;
            }
        }

        public static string HandleOL(Element el)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ol>");
            string[] ListItems = el.Content.Split(',');
            foreach (string Item in ListItems)
            {
                sb.Append("<li>");
                sb.Append(Item);
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ol>");
            return sb.ToString();
        }

        
        public static string HandleUL(Element el)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ul>");
            string[] ListItems = el.Content.Split(',');
            foreach (string Item in ListItems)
            {
                sb.Append("<li>");
                sb.Append(Item);
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        public static string HandleSubElements(Element el)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Element subElement in el.SubElements)
            {
                sb.AppendLine(HandleElement(subElement));
            }
            return sb.ToString();
        }

    }
}