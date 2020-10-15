using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaripSozluk.Common.ViewModels
{
    //public class ApiRowVM
    //{
    //    public List<ApiVM> ApiVMs { get; set; }
    //}


    public class ApiRowVM
    {
        [Required(ErrorMessage ="Metin alanı boş bırakılamaz")]
        [Display(Name ="Metin")]
        public string searchText { get; set; }
        [Required(ErrorMessage = "Seçenek boş bırakılamaz")]
        public int option { get; set; }
        public int start { get; set; }
        public int num_found { get; set; }
        public int numFound { get; set; }
        public ApiVM[] docs { get; set; }
    }

    public class Doc
    {
        //public string[] author_name { get; set; }
        //public string title { get; set; }
        //public string[] publisher { get; set; }
        //public int[] publish_year { get; set; }
        //public int first_publish_year { get; set; }
        //public string[] isbn { get; set; }


        //public string title_suggest { get; set; }
        //public string[] edition_key { get; set; }
        //public int cover_i { get; set; }
      
        //public bool has_fulltext { get; set; }
        //public string[] id_depósito_legal { get; set; }
        //public string[] text { get; set; }
      
        //public string[] ia { get; set; }
        //public string[] ia_loaded_id { get; set; }
        //public string[] seed { get; set; }
        //public string[] id_librarything { get; set; }
        //public string[] id_google { get; set; }
        //public string[] contributor { get; set; }
        //public string[] author_key { get; set; }
        //public string[] subject { get; set; }
        //public string[] ddc { get; set; }
        //public string[] place { get; set; }
        //public string type { get; set; }
        //public int ebook_count_i { get; set; }
        //public string[] publish_place { get; set; }
        //public string[] ia_box_id { get; set; }
        //public int edition_count { get; set; }
        //public string[] id_libris { get; set; }
        //public string[] lcc { get; set; }
        //public string key { get; set; }
        //public string[] id_alibris_id { get; set; }
        //public string[] id_goodreads { get; set; }
        //public bool public_scan_b { get; set; }
        //public string[] id_overdrive { get; set; }
     
        //public string[] id_amazon { get; set; }
        //public string[] language { get; set; }
        //public string[] lccn { get; set; }
        //public int last_modified_i { get; set; }
        //public string[] id_bcid { get; set; }
        //public string[] author_alternative_name { get; set; }
        //public string cover_edition_key { get; set; }
        //public string[] first_sentence { get; set; }
        //public string[] person { get; set; }

        //public string[] publish_date { get; set; }
        //public string[] id_dnb { get; set; }
        //public string[] id_wikidata { get; set; }
        //public string[] oclc { get; set; }
        //public string[] id_amazon_ca_asin { get; set; }
        //public string[] id_amazon_it_asin { get; set; }
        //public string[] id_amazon_co_uk_asin { get; set; }
        //public string[] id_amazon_de_asin { get; set; }
        //public string[] id_nla { get; set; }
        //public string[] id_paperback_swap { get; set; }
        //public string[] time { get; set; }
        //public string[] id_british_national_bibliography { get; set; }
        //public string[] id_scribd { get; set; }
        //public string[] id_british_library { get; set; }
        //public string[] id_bibliothèque_nationale_de_france_bnf { get; set; }
        //public string[] id_hathi_trust { get; set; }
        //public string[] id_canadian_national_library_archive { get; set; }
        //public string lending_identifier_s { get; set; }
        //public string ia_collection_s { get; set; }
        //public string lending_edition_s { get; set; }
        //public string printdisabled_s { get; set; }
        //public string subtitle { get; set; }
    }

}
