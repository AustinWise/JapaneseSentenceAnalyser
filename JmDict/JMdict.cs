﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class JMdict {
    
    private entry[] entryField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("entry")]
    public entry[] entry {
        get {
            return this.entryField;
        }
        set {
            this.entryField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class entry {
    
    private string ent_seqField;
    
    private k_ele[] k_eleField;
    
    private r_ele[] r_eleField;
    
    private sense[] senseField;
    
    /// <remarks/>
    public string ent_seq {
        get {
            return this.ent_seqField;
        }
        set {
            this.ent_seqField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("k_ele")]
    public k_ele[] k_ele {
        get {
            return this.k_eleField;
        }
        set {
            this.k_eleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("r_ele")]
    public r_ele[] r_ele {
        get {
            return this.r_eleField;
        }
        set {
            this.r_eleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("sense")]
    public sense[] sense {
        get {
            return this.senseField;
        }
        set {
            this.senseField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class k_ele {
    
    private string kebField;
    
    private string[] ke_infField;
    
    private string[] ke_priField;
    
    /// <remarks/>
    public string keb {
        get {
            return this.kebField;
        }
        set {
            this.kebField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ke_inf")]
    public string[] ke_inf {
        get {
            return this.ke_infField;
        }
        set {
            this.ke_infField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ke_pri")]
    public string[] ke_pri {
        get {
            return this.ke_priField;
        }
        set {
            this.ke_priField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class r_ele {
    
    private string rebField;
    
    private string re_nokanjiField;
    
    private string[] re_restrField;
    
    private string[] re_infField;
    
    private string[] re_priField;
    
    /// <remarks/>
    public string reb {
        get {
            return this.rebField;
        }
        set {
            this.rebField = value;
        }
    }
    
    /// <remarks/>
    public string re_nokanji {
        get {
            return this.re_nokanjiField;
        }
        set {
            this.re_nokanjiField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("re_restr")]
    public string[] re_restr {
        get {
            return this.re_restrField;
        }
        set {
            this.re_restrField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("re_inf")]
    public string[] re_inf {
        get {
            return this.re_infField;
        }
        set {
            this.re_infField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("re_pri")]
    public string[] re_pri {
        get {
            return this.re_priField;
        }
        set {
            this.re_priField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class sense {
    
    private string[] stagkField;
    
    private string[] stagrField;
    
    private string[] posField;
    
    private string[] xrefField;
    
    private string[] antField;
    
    private string[] fieldField;
    
    private string[] miscField;
    
    private string[] s_infField;
    
    private lsource[] lsourceField;
    
    private string[] dialField;
    
    private gloss[] glossField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("stagk")]
    public string[] stagk {
        get {
            return this.stagkField;
        }
        set {
            this.stagkField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("stagr")]
    public string[] stagr {
        get {
            return this.stagrField;
        }
        set {
            this.stagrField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("pos")]
    public string[] pos {
        get {
            return this.posField;
        }
        set {
            this.posField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("xref")]
    public string[] xref {
        get {
            return this.xrefField;
        }
        set {
            this.xrefField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ant")]
    public string[] ant {
        get {
            return this.antField;
        }
        set {
            this.antField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("field")]
    public string[] field {
        get {
            return this.fieldField;
        }
        set {
            this.fieldField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("misc")]
    public string[] misc {
        get {
            return this.miscField;
        }
        set {
            this.miscField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("s_inf")]
    public string[] s_inf {
        get {
            return this.s_infField;
        }
        set {
            this.s_infField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("lsource")]
    public lsource[] lsource {
        get {
            return this.lsourceField;
        }
        set {
            this.lsourceField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("dial")]
    public string[] dial {
        get {
            return this.dialField;
        }
        set {
            this.dialField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("gloss")]
    public gloss[] gloss {
        get {
            return this.glossField;
        }
        set {
            this.glossField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class lsource {
    
    private string langField;
    
    private string ls_typeField;
    
    private string ls_waseiField;
    
    private string valueField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
    public string lang {
        get {
            return this.langField;
        }
        set {
            this.langField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ls_type {
        get {
            return this.ls_typeField;
        }
        set {
            this.ls_typeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ls_wasei {
        get {
            return this.ls_waseiField;
        }
        set {
            this.ls_waseiField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value {
        get {
            return this.valueField;
        }
        set {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class gloss {
    
    private string[] itemsField;
    
    private string[] textField;
    
    private string langField;
    
    private string g_gendField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("pri")]
    public string[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string[] Text {
        get {
            return this.textField;
        }
        set {
            this.textField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
    public string lang {
        get {
            return this.langField;
        }
        set {
            this.langField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string g_gend {
        get {
            return this.g_gendField;
        }
        set {
            this.g_gendField = value;
        }
    }
}
