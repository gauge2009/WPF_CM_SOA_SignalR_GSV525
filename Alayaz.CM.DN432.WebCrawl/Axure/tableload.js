    var dataset=[]
	function loadHxData(dataset){
				var table=$('#example').DataTable( {
				"data": dataset,
				"destroy": true,//设置数据更新
				"columns": [
					 {
		                "mRender": function (data, type, full) {
		                	var sReturn="";
		                	var rs1="";
		                	var rs2="";
		                	var val="0";		               
		                	if(data[0]=="1"){
		                		rs1="checked";
		                		val="1";
		                	}if(data[1]=="0"){
		                		rs2="disabled";
		                	}

		                	sReturn= '<input type="checkbox" name="checkbox1" '+rs1+' '+rs2+' value="'+val+'" />';
		                	                   
		                    return sReturn;
		                },
		                "sClass": "checkboxes",
		                "width":"45px"
		             },		
		            { "title": "状态" }	,
					{ "title": "发票代码"},
	 				{ "title": "发票号码" },
	 				{ "title": "开票日期" },
	 				{ "title": "销方税号" },
	 				{ "title": "金额" },
	 				{ "title": "税额" },
	 				{ "title": "发票状态代码" }	,
	 				{"title": "勾选结果代码" },
	 				{"title": "来源代码" },
	 				{ "title": "来源" },
	 				{ "title": "发票状态" } ,
	 				{ "title": "勾选标志" } ,
	 				{"title":"操作时间"}				
	 				 				
				],	
				"oLanguage": {
					"sLengthMenu": "每页显示 _MENU_ 条记录",
					"sSearch":"搜索:",
					"sInfo": "显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
					"oPaginate": {
						"sPrevious": "上一页",
						"sNext": "下一页"
					},
					"sInfoEmpty":"显示 0 到 0 共 0 条记录",
					"sInfoFiltered":"(查询来自 _MAX_ 条记录)",
					"sZeroRecords": "没找到记录"		
				},
				"searching":true,//设置是否显示搜索
				"aLengthMenu": [ 10,15,20],
				"columnDefs":[{
		                 orderable:false,//禁用排序
		                 targets:[0]   //指定的列
		             },{ 
			          "targets":[1,8,9,10], //隐藏第7,8列，从第0列开始   
					  "visible": false    
				    } 
			    ],
			    "initComplete": function () {
		            var api = this.api();            
		            api.$('tr').click( function () {

						var elemckeck1="input[name='checkbox1']:eq("+(this.rowIndex-1)+")";
		            	if($(elemckeck1).is(":focus")){
			            	if($(elemckeck1).is(":checked")==true){
			            		$(this).find("input:eq(0)").val("1");
			            	} else{
			            		$(this).find("input:eq(0)").val("0");
			            		$('#a1').prop("checked",false);
			            	}
						}
		            } );
		        }
			} );		    
	}


	function loadUnConfirmCxData(dataset){
				var table=$('#example1').DataTable( {
				"data": dataset,
				"destroy": true,//设置数据更新
				"columns": [
					{
		                "mRender": function (data, type, full) {
		                	var sReturn="";
		                	var rs1="";
		                	var rs2="";
		                	var val="0";		               
		                	if(data[0]=="1"){
		                		rs1="checked";
		                		val="1";
		                	}if(data[1]=="0"){
		                		rs2="disabled";
		                	}

		                	sReturn= '<input type="checkbox" name="checkbox1" '+rs1+' '+rs2+' value="'+val+'" />';
		                	                   
		                    return sReturn;
		                },
		                "sClass": "checkboxes",
		                "width":"15px"
		            },	
		            { "title": "状态" }	,
					{ "title": "发票代码"},
	 				{ "title": "发票号码" },
	 				{ "title": "开票日期" },
	 				{ "title": "销方税号" },
	 				{ "title": "金额" },
	 				{ "title": "税额" },
	 				{ "title": "发票状态代码" }	,
	 				{"title": "勾选结果代码" },
	 				{"title": "来源代码" },
	 				{ "title": "来源" },
	 				{ "title": "发票状态" } ,
	 				{ "title": "勾选标志" }	,
	 				{"title":"操作时间"}		
	 				 				
				],	
				"oLanguage": {
					"sLengthMenu": "每页显示 _MENU_ 条记录",
					"sSearch":"搜索:",
					"sInfo": "显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
					"oPaginate": {
						"sPrevious": "上一页",
						"sNext": "下一页"
					},
					"sInfoEmpty":"显示 0 到 0 共 0 条记录",
					"sInfoFiltered":"(查询来自 _MAX_ 条记录)",
					"sZeroRecords": "没找到记录"		
				},
				"searching":true,//设置是否显示搜索
				"aLengthMenu": [ 10,15,20],
				"columnDefs":[{ 
			          "targets":[0,1,8,9,10], //隐藏第7,8列，从第0列开始   
					  "visible": false    
				    } 
			    ]
			} );		    
	}
	
	function loadConfirmCxData(dataset){
				var table=$('#example').DataTable( {
				"data": dataset,
				"destroy": true,//设置数据更新
				"columns": [
					{
		                "mRender": function (data, type, full) {
		                	var sReturn="";
		                	var rs1="";
		                	var rs2="";
		                	var val="0";		               
		                	if(data[0]=="1"){
		                		rs1="checked";
		                		val="1";
		                	}if(data[1]=="0"){
		                		rs2="disabled";
		                	}

		                	sReturn= '<input type="checkbox" name="checkbox1" '+rs1+' '+rs2+' value="'+val+'" />';
		                	                   
		                    return sReturn;
		                },
		                "sClass": "checkboxes",
		                "width":"15px"
		            },		
		            { "title": "状态" }	,
					{ "title": "发票代码"},
	 				{ "title": "发票号码" },
	 				{ "title": "开票日期" },
	 				{ "title": "销方税号" },
	 				{ "title": "金额" },
	 				{ "title": "税额" },
	 				{ "title": "发票状态代码" }	,
	 				{"title": "勾选结果代码" },
	 				{"title": "来源代码" },
	 				{ "title": "来源" },
	 				{ "title": "发票状态" } ,
	 				{ "title": "勾选标志" }, 
	 				{ "title": "确认月份" } 
	 				 				
				],	
				"oLanguage": {
					"sLengthMenu": "每页显示 _MENU_ 条记录",
					"sSearch":"搜索:",
					"sInfo": "显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
					"oPaginate": {
						"sPrevious": "上一页",
						"sNext": "下一页"
					},
					"sInfoEmpty":"显示 0 到 0 共 0 条记录",
					"sInfoFiltered":"(查询来自 _MAX_ 条记录)",
					"sZeroRecords": "没找到记录"		
				},
				"searching":true,//设置是否显示搜索
				"aLengthMenu": [ 10,15,20],
				"columnDefs":[{ 
			          "targets":[0,1,8,9,10,13], //隐藏第7,8列，从第0列开始   
					  "visible": false    
				    } 
			    ]
			} );		    
	}

	//重新组织数据
	function createDateSet(data){
		var tempSet=[];
		var check=[];
		var zt="0";
		if(data[6]=="0"){
			if(data[8]=="1"){
				if(data[7]=="0"){
					check.push("0");
					check.push("1");
				}else if(data[7]=="1"){
					check.push("1");
					check.push("1");
					zt="1";
				}else if(data[7]=="2"){
					check.push("1");
					check.push("0");
					zt="1";
				}else{
					check.push("0");
					check.push("0");
				}
			}else{
				check.push("1");
				check.push("0");
				zt="1";
			}
			
		}else{
			check.push("0");
			check.push("0");
		}
		tempSet.push(check);
		tempSet.push(zt);
		for(var i=0;i<=8;i++){
			tempSet.push(data[i]);
		}
		if(data[8]=="2"){
			tempSet.push("认证");
		}else if(data[8]=="1"){
			tempSet.push("底账");
		}else{
			tempSet.push("其它");
		}
		var zwzt="";
		if(data[6]=="0"){
			zwzt="正常";
		}else if(data[6]=="1"){
			zwzt="失控";
		}else if(data[6]=="2"){
			zwzt="作废";
		}else if(data[6]=="3"){
			zwzt="红冲";
		}else if(data[6]=="4"){
			zwzt="异常";
		}
		tempSet.push(zwzt);
		var gxqk="";
		if(data[7]=="0"){
			gxqk="未勾选";
		}else if(data[7]=="1"){
			gxqk="已勾选";
		}else if(data[7]=="2"){
			gxqk="已确认";
		}
		tempSet.push(gxqk);		

		tempSet.push(data[9]=="null"?"未操作":data[9]);

		return tempSet;
	}
