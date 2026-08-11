```json
{
  "create": {
    "create new <form caption>": {
      "cmdToken": 1,
      "command": "create new <form caption>",
      "commandGroup": "create",
      "action": "opens tstruct/form in new mode to enter data.",
      "prompts": [
        {
          "cmdToken": 1,
          "wordPos": 3,
          "prompt": "tstruct name",
          "promptSource": "TStructList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create tstruct <transid> <form caption>": {
      "cmdToken": 2,
      "command": "create tstruct <transid> <form caption>",
      "commandGroup": "create",
      "action": "open the tstruct definition interface in new mode.",
      "prompts": [
        {
          "cmdToken": 2,
          "wordPos": 3,
          "prompt": "tstruct name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create ads <adsname>": {
      "cmdToken": 3,
      "command": "create ads <adsname>",
      "commandGroup": "create",
      "action": "open ads definition form in new mode",
      "prompts": [
        {
          "cmdToken": 3,
          "wordPos": 3,
          "prompt": "ads name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create page <pagename>": {
      "cmdToken": 4,
      "command": "create page <pagename>",
      "commandGroup": "create",
      "action": "popups page definition window",
      "prompts": [
        {
          "cmdToken": 4,
          "wordPos": 3,
          "prompt": "page name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create script <tstruct/iview> <name>": {
      "cmdToken": 5,
      "command": "create script <tstruct/iview> <name>",
      "commandGroup": "create",
      "action": "popups script definition window",
      "prompts": [
        {
          "cmdToken": 5,
          "wordPos": 3,
          "prompt": "object type",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "tstruct,iview"
        },
        {
          "cmdToken": 5,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": "AllObjectList",
          "promptParams": 3,
          "promptValues": null
        },
        {
          "cmdToken": 5,
          "wordPos": 5,
          "prompt": "script name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create card <cardname>": {
      "cmdToken": 6,
      "command": "create card <cardname>",
      "commandGroup": "create",
      "action": "popup card definition window",
      "prompts": [
        {
          "cmdToken": 6,
          "wordPos": 3,
          "prompt": "card name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create user <username>": {
      "cmdToken": 7,
      "command": "create user <username>",
      "commandGroup": "create",
      "action": "popup user creation form",
      "prompts": [
        {
          "cmdToken": 7,
          "wordPos": 3,
          "prompt": "user name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create role <rolename>": {
      "cmdToken": 8,
      "command": "create role <rolename>",
      "commandGroup": "create",
      "action": "popup role creation form",
      "prompts": [
        {
          "cmdToken": 8,
          "wordPos": 3,
          "prompt": "role name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create dimension <dimensionname> <value>": {
      "cmdToken": 9,
      "command": "create dimension <dimensionname> <value>",
      "commandGroup": "create",
      "action": "popup dimension creation form",
      "prompts": [
        {
          "cmdToken": 9,
          "wordPos": 3,
          "prompt": "dimension name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 9,
          "wordPos": 4,
          "prompt": "dimension value",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create usergroup <groupname> <usernames>": {
      "cmdToken": 10,
      "command": "create usergroup <groupname> <usernames>",
      "commandGroup": "create",
      "action": "popup user group creation form if user names are not provided",
      "prompts": [
        {
          "cmdToken": 10,
          "wordPos": 3,
          "prompt": "group name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 10,
          "wordPos": 4,
          "prompt": "user names",
          "promptSource": "UserList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "create actor <actorname>": {
      "cmdToken": 11,
      "command": "create actor <actorname>",
      "commandGroup": "create",
      "action": "popup actor creation form",
      "prompts": [
        {
          "cmdToken": 11,
          "wordPos": 3,
          "prompt": "actor name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  },
  "edit": {
    "edit data <transid> <keyfield> <keyvalue>": {
      "cmdToken": 12,
      "command": "edit data <transid> <keyfield> <keyvalue>",
      "commandGroup": "edit",
      "action": "popup tstruct form with given data.",
      "prompts": [
        {
          "cmdToken": 12,
          "wordPos": 3,
          "prompt": "form name",
          "promptSource": "TStructList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 12,
          "wordPos": 4,
          "prompt": "search field",
          "promptSource": "FieldList",
          "promptParams": 3,
          "promptValues": null
        },
        {
          "cmdToken": 12,
          "wordPos": 5,
          "prompt": "search value",
          "promptSource": "FieldValueList",
          "promptParams": 4,
          "promptValues": null
        }
      ]
    },
    "edit <tstruct/iview/ads/page/card> <name>": {
      "cmdToken": 13,
      "command": "edit <tstruct/iview/ads/page/card> <name>",
      "commandGroup": "edit",
      "action": "popup relevant definition form",
      "prompts": [
        {
          "cmdToken": 13,
          "wordPos": 3,
          "prompt": "type",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "tstruct,iview,ads,page,card"
        },
        {
          "cmdToken": 13,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "edit user <username>": {
      "cmdToken": 14,
      "command": "edit user <username>",
      "commandGroup": "edit",
      "action": "popup user definition form with given user",
      "prompts": [
        {
          "cmdToken": 14,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "UserList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "edit role <rolename>": {
      "cmdToken": 15,
      "command": "edit role <rolename>",
      "commandGroup": "edit",
      "action": "popup role definition form with given role",
      "prompts": [
        {
          "cmdToken": 15,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "Rolelist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "edit usergroup <usergroupname>": {
      "cmdToken": 16,
      "command": "edit usergroup <usergroupname>",
      "commandGroup": "edit",
      "action": "popup user group creation form in edit mode",
      "prompts": [
        {
          "cmdToken": 16,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "UserGroupList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "edit <actorname>": {
      "cmdToken": 17,
      "command": "edit <actorname>",
      "commandGroup": "edit",
      "action": "popup actor form if actorname given else show actor listing",
      "prompts": [
        {
          "cmdToken": 17,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "ActorList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "edit <dimensionname>": {
      "cmdToken": 18,
      "command": "edit <dimensionname>",
      "commandGroup": "edit",
      "action": "popup dimension value list of given dimension.",
      "prompts": [
        {
          "cmdToken": 18,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "DimensionList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  },
  "view": {
    "view data <transid> <keyfield> <keyvalue>": {
      "cmdToken": 19,
      "command": "view data <transid> <keyfield> <keyvalue>",
      "commandGroup": "view",
      "action": "popup datapage with given data. If keyfield & value not provided, show the data listing page.",
      "prompts": [
        {
          "cmdToken": 19,
          "wordPos": 3,
          "prompt": "form name",
          "promptSource": "TStructList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 19,
          "wordPos": 4,
          "prompt": "search field",
          "promptSource": "FieldList",
          "promptParams": 3,
          "promptValues": null
        },
        {
          "cmdToken": 19,
          "wordPos": 5,
          "prompt": "search value",
          "promptSource": "FieldValueList",
          "promptParams": 4,
          "promptValues": null
        }
      ]
    },
    "view report <iviewname>": {
      "cmdToken": 20,
      "command": "view report <iviewname>",
      "commandGroup": "view",
      "action": "popup iview page with given iview result",
      "prompts": [
        {
          "cmdToken": 20,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "IViewList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view list <adsname>": {
      "cmdToken": 21,
      "command": "view list <adsname>",
      "commandGroup": "view",
      "action": "popup smart view page with given ads resullt",
      "prompts": [
        {
          "cmdToken": 21,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "AdsList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view inbox": {
      "cmdToken": 22,
      "command": "view inbox",
      "commandGroup": "view",
      "action": "popup inbox page",
      "prompts": [
        {
          "cmdToken": 22,
          "wordPos": null,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view source <tstruct/iview/field/genmap/mdmap/fillgrid/column/dc/script> <name>": {
      "cmdToken": 23,
      "command": "view source <tstruct/iview/field/genmap/mdmap/fillgrid/column/dc/script> <name>",
      "commandGroup": "view",
      "action": "popup the defintion of the object as a name-value pair",
      "prompts": [
        {
          "cmdToken": 23,
          "wordPos": 3,
          "prompt": "type",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "tstruct,iview,ads,page,card"
        },
        {
          "cmdToken": 23,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": "AllObjectList",
          "promptParams": 3,
          "promptValues": null
        }
      ]
    },
    "view user <username>": {
      "cmdToken": 24,
      "command": "view user <username>",
      "commandGroup": "view",
      "action": "display the user details along with permissions",
      "prompts": [
        {
          "cmdToken": 24,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "UserList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view role <rolename>": {
      "cmdToken": 25,
      "command": "view role <rolename>",
      "commandGroup": "view",
      "action": "display the role details along with permissions",
      "prompts": [
        {
          "cmdToken": 25,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "Rolelist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view actor <actorname>": {
      "cmdToken": 26,
      "command": "view actor <actorname>",
      "commandGroup": "view",
      "action": null,
      "prompts": [
        {
          "cmdToken": 26,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "ActorList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view usergroup <usergroupname>": {
      "cmdToken": 27,
      "command": "view usergroup <usergroupname>",
      "commandGroup": "view",
      "action": null,
      "prompts": [
        {
          "cmdToken": 27,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "UserGroupList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view dimensions <dimensionname>": {
      "cmdToken": 28,
      "command": "view dimensions <dimensionname>",
      "commandGroup": "view",
      "action": "list the dimensions & its values",
      "prompts": [
        {
          "cmdToken": 28,
          "wordPos": 3,
          "prompt": "name",
          "promptSource": "DimensionList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "view dbconsole": {
      "cmdToken": 29,
      "command": "view dbconsole",
      "commandGroup": "view",
      "action": "opens the db console",
      "prompts": [
        {
          "cmdToken": 29,
          "wordPos": null,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  },
  "send": {
    "send message <touser> <text>": {
      "cmdToken": 30,
      "command": "send message <touser> <text>",
      "commandGroup": "send",
      "action": "sends message to the given user",
      "prompts": [
        {
          "cmdToken": 30,
          "wordPos": 3,
          "prompt": "user name",
          "promptSource": "UserList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 30,
          "wordPos": 4,
          "prompt": "message text",
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "send data <touser> <keyfield> <keyvalue> <printformname>": {
      "cmdToken": 31,
      "command": "send data <touser> <keyfield> <keyvalue> <printformname>",
      "commandGroup": "send",
      "action": "sends data as pdf to user",
      "prompts": [
        {
          "cmdToken": 31,
          "wordPos": 3,
          "prompt": "user name",
          "promptSource": "UserList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 31,
          "wordPos": 4,
          "prompt": "transid",
          "promptSource": "TStructList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 31,
          "wordPos": 5,
          "prompt": "search field",
          "promptSource": "FieldList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 31,
          "wordPos": 6,
          "prompt": "search value",
          "promptSource": "FieldValueList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 31,
          "wordPos": 7,
          "prompt": "print form name",
          "promptSource": "printformlist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "send ads <touser> <adsname> <excel/pdf>": {
      "cmdToken": 32,
      "command": "send ads <touser> <adsname> <excel/pdf>",
      "commandGroup": "send",
      "action": "sends a pdf or excel of the ads result",
      "prompts": [
        {
          "cmdToken": 32,
          "wordPos": 3,
          "prompt": "user name",
          "promptSource": "UserList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 32,
          "wordPos": 4,
          "prompt": "ads name",
          "promptSource": "AdsList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 32,
          "wordPos": 5,
          "prompt": "file type",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "excel,pdf"
        }
      ]
    },
    "send iview <touser> <iviewname> <excel/pdf>": {
      "cmdToken": 33,
      "command": "send iview <touser> <iviewname> <excel/pdf>",
      "commandGroup": "send",
      "action": "accepts param values and sends iview result as excel or pdf",
      "prompts": [
        {
          "cmdToken": 33,
          "wordPos": 3,
          "prompt": "touser",
          "promptSource": "UserList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 33,
          "wordPos": 4,
          "prompt": "ivew name",
          "promptSource": "IViewList",
          "promptParams": null,
          "promptValues": null
        },
        {
          "cmdToken": 33,
          "wordPos": 5,
          "prompt": "file type",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "excel,pdf"
        }
      ]
    }
  },
  "configure": {
    "configure peg <pegname>": {
      "cmdToken": 34,
      "command": "configure peg <pegname>",
      "commandGroup": "configure",
      "action": "opens peg definition in new or edit mode",
      "prompts": [
        {
          "cmdToken": 34,
          "wordPos": 3,
          "prompt": "peg name",
          "promptSource": "PegList",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure notification <form/periodic> <name>": {
      "cmdToken": 35,
      "command": "configure notification <form/periodic> <name>",
      "commandGroup": "configure",
      "action": "opens the relevant notification defintion window",
      "prompts": [
        {
          "cmdToken": 35,
          "wordPos": 3,
          "prompt": "notification type",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "form,scheduled"
        },
        {
          "cmdToken": 35,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": "NotificatonNameList",
          "promptParams": 3,
          "promptValues": null
        }
      ]
    },
    "configure job <jobname>": {
      "cmdToken": 36,
      "command": "configure job <jobname>",
      "commandGroup": "configure",
      "action": "opens the job definition form in edit or new mode",
      "prompts": [
        {
          "cmdToken": 36,
          "wordPos": 3,
          "prompt": "job name",
          "promptSource": "Jobnameslist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure printform <printformname>": {
      "cmdToken": 37,
      "command": "configure printform <printformname>",
      "commandGroup": "configure",
      "action": "opens the print form defintion for in new/edit mode",
      "prompts": [
        {
          "cmdToken": 37,
          "wordPos": 3,
          "prompt": "print form name",
          "promptSource": "printformlist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure api <apiname>": {
      "cmdToken": 38,
      "command": "configure api <apiname>",
      "commandGroup": "configure",
      "action": "opens the api configuration window",
      "prompts": [
        {
          "cmdToken": 38,
          "wordPos": 3,
          "prompt": "api name",
          "promptSource": "apinameslist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure rule <rulename>": {
      "cmdToken": 39,
      "command": "configure rule <rulename>",
      "commandGroup": "configure",
      "action": "opens the rule definition form in new or edit mode",
      "prompts": [
        {
          "cmdToken": 39,
          "wordPos": 3,
          "prompt": "rule name",
          "promptSource": "rulenameslist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure appvar <varname> <const/dbvar/variable>": {
      "cmdToken": 40,
      "command": "configure appvar <varname> <const/dbvar/variable>",
      "commandGroup": "configure",
      "action": "opens the variable definition window",
      "prompts": [
        {
          "cmdToken": 40,
          "wordPos": null,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure properties": {
      "cmdToken": 41,
      "command": "configure properties",
      "commandGroup": "configure",
      "action": "opens the settings window",
      "prompts": [
        {
          "cmdToken": 41,
          "wordPos": null,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure devoptions": {
      "cmdToken": 42,
      "command": "configure devoptions",
      "commandGroup": "configure",
      "action": "lists all the devoptions and lets user create new options",
      "prompts": [
        {
          "cmdToken": 42,
          "wordPos": null,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure permission <role/user> <name> <tstruct/iview/page/ads> <name>": {
      "cmdToken": 43,
      "command": "configure permission <role/user> <name> <tstruct/iview/page/ads> <name>",
      "commandGroup": "configure",
      "action": "open the permission tstruct for given user/role & object",
      "prompts": [
        {
          "cmdToken": 43,
          "wordPos": 3,
          "prompt": "role or user",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "role/user"
        },
        {
          "cmdToken": 43,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": "roleanduserlist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure access <role/user> <name>": {
      "cmdToken": 44,
      "command": "configure access <role/user> <name>",
      "commandGroup": "configure",
      "action": "open the iview that lists the access for given user/role",
      "prompts": [
        {
          "cmdToken": 44,
          "wordPos": 3,
          "prompt": "role or user",
          "promptSource": null,
          "promptParams": null,
          "promptValues": "role/user"
        },
        {
          "cmdToken": 44,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": "roleanduserlist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    },
    "configure server <name>": {
      "cmdToken": 45,
      "command": "configure server <name>",
      "commandGroup": "configure",
      "action": "open the server confiuration form",
      "prompts": [
        {
          "cmdToken": 45,
          "wordPos": 3,
          "prompt": "server name",
          "promptSource": "servernamelist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  },
  "upload": {
    "upload data": {
      "cmdToken": 46,
      "command": "upload data",
      "commandGroup": "upload",
      "action": "open the import wizard",
      "prompts": [
        {
          "cmdToken": 46,
          "wordPos": null,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  },
  "download": {
    "download data": {
      "cmdToken": 47,
      "command": "download data",
      "commandGroup": "download",
      "action": "open a form that will export data into an excel",
      "prompts": [
        {
          "cmdToken": 47,
          "wordPos": null,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  },
  "publish": {
    "publish structure <tstruct/iview/ads/page/peg/notification/rule> <name>": {
      "cmdToken": 48,
      "command": "publish structure <tstruct/iview/ads/page/peg/notification/rule> <name>",
      "commandGroup": "publish",
      "action": "publish the tstruct to run time",
      "prompts": [
        {
          "cmdToken": 48,
          "wordPos": 3,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": "tstruct,iview,ads,page,card,peg,notification,rule"
        },
        {
          "cmdToken": 48,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": "objectnameslist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  },
  "promote": {
    "promote object <toserver> <tstruct/iview/ads/peg/notification/job/printform/page/api/rule/appvar/props/devoptions/user/role/permission/access> <name>": {
      "cmdToken": 49,
      "command": "promote object <toserver> <tstruct/iview/ads/peg/notification/job/printform/page/api/rule/appvar/props/devoptions/user/role/permission/access> <name>",
      "commandGroup": "promote",
      "action": "promotes the given object to the given server",
      "prompts": [
        {
          "cmdToken": 49,
          "wordPos": 3,
          "prompt": null,
          "promptSource": null,
          "promptParams": null,
          "promptValues": "tstruct,iview,ads,page,card,peg,notification,rule"
        },
        {
          "cmdToken": 49,
          "wordPos": 4,
          "prompt": "name",
          "promptSource": "objectnameslist",
          "promptParams": null,
          "promptValues": null
        }
      ]
    }
  }
}
```
