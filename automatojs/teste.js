// set up SVG for D3
var width  = 960,
    height = 500,
    colors = d3.scale.category10();

var svg = d3.select('place')
    .append('svg')
	.attr('class','local')
    //.attr('oncontextmenu', 'return false;')
    .attr('width', width)
    .attr('height', height);

//Elementos
var nodes = [{id: 1, end: false, start : true},
			{id: 2, end: true, start : false}],
	//nodes =[],
    lastNodeId = 2,
	links = [];
	//links = [{source: nodes[0], target:nodes[0], right : true, need : ['a'], id: this.need}];

// init D3 force layout
var force = d3.layout.force()
    .nodes(nodes)
    .links(links)
    .size([width, height])
    .linkDistance(150)
    .charge(-500)
    .on('tick', tick)

// define arrow markers for graph links
svg.append('svg:defs').append('svg:marker')
    .attr('id', 'end-arrow')
    .attr('viewBox', '0 -5 10 10')
    .attr('refX', 15)
    .attr("refY", 0)
    .attr('markerWidth', 6)
    .attr('markerHeight', 6)
    .attr('orient', 'auto')
    .append('svg:path')
    .attr('d', 'M0,-5L10,0L0,5')
    .attr('fill', '#000');

svg.append('svg:defs').append('svg:marker')
    .attr('id', 'start-arrow')
    .attr('viewBox', '0 -5 10 10')
    .attr('refX', -4)  //Posição da Marca
    .attr("refY", 0)
    .attr('markerWidth', 6)
    .attr('markerHeight', 6)
    .attr('orient', 'auto')
    .append('svg:path')
    .attr('d', 'M10,-5L0,0L10,5')
    .attr('fill', '#000');

// line displayed when dragging new nodes
var drag_line = svg.append('svg:path')
    .attr('class', 'link dragline hidden')
    .attr('d', 'M0,0L0,0');

// handles to link and node element groups
var path = svg.append('svg:g').selectAll('path'),
    circle = svg.append('svg:g').selectAll('g'),
    linkText = svg.append('svg:g').selectAll('link');

// mouse event vars
var selected_node = null,
    selected_link = null,
    mousedown_link = null,
    mousedown_node = null,
    mouseup_node = null;

var drag = force.drag()
   .on("dragstart", dragstart);

function resetMouseVars() {
    mousedown_node = null;
    mouseup_node = null;
    mousedown_link = null;
}

//Curva da Linha
function tick() {
    path.attr("d", function(d) {
        var x1 = d.source.x,
            y1 = d.source.y,
            x2 = d.target.x,
            y2 = d.target.y,
            dx = x2 - x1,
            dy = y2 - y1,
            dr = Math.sqrt(dx * dx + dy * dy),

            // Defaults for normal edge.
            drx = dr,
            dry = dr,
            xRotation = 0, // degrees
            largeArc = 0, // 1 or 0
            sweep = 1; // 1 or 0

        // Self edge.
        if ( x1 === x2 && y1 === y2 ) {
            // Fiddle with this angle to get loop oriented.
            xRotation = -85;

            // Needs to be 1.
            largeArc = 1;
			//0 pra ser inicial

            // Change sweep to change orientation of loop.
            //sweep = 0;

            // Make drx and dry different to get an ellipse
            // instead of a circle.
            drx = 30;
            dry = 20;

            // For whatever reason the arc collapses to a point if the beginning
            // and ending points of the arc are the same, so kludge it.
            x2 = x2 + 4;
            y2 = y2 + 1;
        }

        return "M" + x1 + "," + y1 + "A" + drx + "," + dry + " " + xRotation + "," + largeArc + "," + sweep + " " + x2 + "," + y2;
    });
    circle.attr("transform", function(d) {return "translate(" + d.x + "," + d.y + ")"; });

    linkText
        .attr("x", function(d) {
            if (d.source != d.target)return (d.source.x + (d.target.x - d.source.x) * 0.5);
            else return ((d.source.x + (d.target.x - d.source.x) * 0.5)+30);
        })
        .attr("y", function(d) {
            if (d.source != d.target )return (d.source.y + (d.target.y - d.source.y) * 0.5);
            else  return ((d.source.y + (d.target.y - d.source.y) * 0.5) -50);
        })
}


// update graph (called when needed)
function restart() {
    exibe();
	
    // path (link) group
    path = path.data(links);

    // update existing links
    path.classed('selected', function(d) { return d === selected_link; })
        .style('marker-end', function(d) { return d.right ? 'url(#end-arrow)' : ''; });

    path.classed('selected', function(d) { return d === selected_link; })
        .style('marker-start', function(d) { return d.left ? 'url(#start-arrow)' : ''; });



    // add new links
    path.enter().append('svg:path')
        .attr('class', 'link')
        .attr('id','a')
        .classed('selected', function(d) { return d === selected_link; })
        .style('marker-start', function(d) { return d.left ? 'url(#start-arrow)' : ''; })
        .style('marker-end', function(d) { return d.right ? 'url(#end-arrow)' : ''; })
        .on('mousedown', function(d) {
            if(d3.event.ctrlKey) return;

            // select link
            mousedown_link = d;
            if(mousedown_link === selected_link) {

                selected_link = null;
            }
            else {
                selected_link = mousedown_link;

            }
            selected_node = null;
            restart();
        })



    // remove old links
    path.exit().remove();

    linkText = linkText.data(links);
    linkText.enter()
        .append("text")
        .data(force.links())
        .text(function(d) { return d.need;})
        .attr("x", function(d) {
            if (d.source != d.target)return (d.source.x + (d.target.x - d.source.x) * 0.5);
            else return ((d.source.x + (d.target.x - d.source.x) * 0.5)+30);
             })
        .attr("y", function(d) {
            if (d.source != d.target )return (d.source.y + (d.target.y - d.source.y) * 0.5);
        else  return ((d.source.y + (d.target.y - d.source.y) * 0.5) -50);
        })
        .attr("dy", ".25em")
        .attr("text-anchor", "middle");

    linkText.exit().remove();

    //REVISAR +30 -50

    // circle (node) group
    // NB: the function arg is crucial here! nodes are known by id, not by index!
    circle = circle.data(nodes, function(d) { return d.id; });

    // update existing nodes (end & selected visual states)
    circle.selectAll('circle')
        //.style('fill', function(d) { return (d === selected_node) ? d3.rgb(colors(d.id)).brighter().toString() : colors(d.id); })
        //.style('fill', 'rgb(255,255,255)')
		.classed('end', function(d) { return d.end; })
		.classed('start', function(d) { return d.start;});

    // add new nodes
    var g = circle.enter().append('svg:g');

    g.append('svg:circle')
        .attr('class', 'node')
        .attr('r', 16)
        //.style('fill', function(d) { return (d === selected_node) ? d3.rgb(colors(d.id)).brighter().toString() : colors(d.id); })
        //.style('fill', 'rgb(255,255,255)')
		.style('stroke', function(d) { return d3.rgb(colors(d.id)).darker().toString(); })
        .classed('end', function(d) { return d.end; })
		.classed('start', function(d) { return d.start; })
        .on('mouseover', function(d) {
            if(!mousedown_node || d === mousedown_node) return;
            // enlarge target node
            d3.select(this).attr('transform', 'scale(1.1)');

        })
        .on('mouseout', function(d) {
            if(!mousedown_node || d === mousedown_node) return;
            // unenlarge target node
            d3.select(this).attr('transform', '');

        })
        .on('mousedown', function(d) {
            //if(d3.event.ctrlKey) return;
			
            // Seleciona o Node
            mousedown_node = d;
            if(mousedown_node === selected_node) selected_node = null;
            else selected_node = mousedown_node;
            selected_link = null;

			if (d3.event.ctrlKey){
			//Desenha a arrow saindo do node
            drag_line
                .style('marker-end', 'url(#end-arrow)')
                .classed('hidden', false)
                .attr('d', 'M' + mousedown_node.x + ',' + mousedown_node.y + 'L' + mousedown_node.x + ',' + mousedown_node.y);
			}
            restart();
			
        })
        .on('mouseup', function(d) {
            if(!mousedown_node) return;

            // needed by FF
            drag_line
                .classed('hidden', true)
                .style('marker-end', '');

            // check for drag-to-self*
            mouseup_node = d;
            //if(mouseup_node === mousedown_node && d3.event.shiftKey) { resetMouseVars(); return; }
			if (!d3.event.ctrlKey){ resetMouseVars(); return;}
            // unenlarge target node
            d3.select(this).attr('transform', '');


//Direção da Linha ( ir e vir)
            var source, target, direction;
            source = mousedown_node;
            target = mouseup_node;
            direction = 'right';


            var link;
            //addLink(link);
            link = links.filter(function(l) {
                return (l.source === source && l.target === target);
            })[0];

            if(link) {
                link[direction] = true;
            } else {
                link = {source: source, target: target, left: false, right: false};
                link[direction] = true;
                links.push(link);
                link.need = prompt();
				if (link.need == ""){
					//link.need = "Vazio";
				}
            }

            // select new link
            selected_link = link;
            selected_node = null;
            restart();
        });

    // show node IDs
    g.append('svg:text')
        .attr('x',0)
        .attr('y', 4)
        .attr('class', 'id')
        .text(function(d) { return 'q'+d.id; });

    // remove old nodes
    circle.exit().remove();



    // set the graph in motion
    force.start();
}



function mousedown() {
    // prevent I-bar on drag
    //d3.event.preventDefault();

    // because :active only works in WebKit?
    svg.classed('active', true);

    //if(d3.event.ctrlKey || mousedown_node || mousedown_link || d3.event.shiftKey ) return;
	if (d3.event.ctrlKey && !mousedown_node){
    // insert new node at point
    var point = d3.mouse(this),
        node = {id: ++lastNodeId, end: false, start : false};
    node.x = point[0];
    node.y = point[1];
	node.fixed = true;//Travar node ao inserir
    nodes.push(node);

    restart();
	}
	else{
		selected_node = mousedown_node;
		
	}
}

function mousemove() {
    if(!mousedown_node) return;

    // update Linha
    drag_line.attr('d', 'M' + mousedown_node.x + ',' + mousedown_node.y + 'L' + d3.mouse(this)[0] + ',' + d3.mouse(this)[1]);

    restart();
}

function mouseup() {
    if(mousedown_node) {
        // hide drag line
        drag_line
            .classed('hidden', true)
            .style('marker-end', '');
    }

    // because :active only works in WebKit?
    svg.classed('active', false);

    // clear mouse event vars
    resetMouseVars();
}

function spliceLinksForNode(node) {
    var toSplice = links.filter(function(l) {
        return (l.source === node || l.target === node);
    });
    toSplice.map(function(l) {
        links.splice(links.indexOf(l), 1);
    });
}

// only respond once per keydown
var lastKeyDown = -1;

function keydown() {
    d3.event.preventDefault();

    if(lastKeyDown !== -1) return;
    lastKeyDown = d3.event.keyCode;

    // ctrl 17, shift 16
    if(d3.event.keyCode === 16) {
        circle.call(drag);
    }

    if(!selected_node && !selected_link) return;
    switch(d3.event.keyCode) {
        case 8: // backspace
        case 46: // delete
            deletenode();
            break;
        case 66: // B
            if(selected_link) {
                // set link direction to both left and right
                selected_link.left = false;
                selected_link.right = true;
            }
            restart();
            break;
        case 76: // L
            if(selected_link) {
                // set link direction to left only
                selected_link.left = true;
                selected_link.right = false;
            }
            restart();
            break;
        case 70: // F
            endnode();
            break;
		case 73 : // I
			initnode();
			break;
		case 74 : //J
			
		break;
    }
}


function removeInit(){
	 for (prop in nodes){
        nodes[prop].start = false;
    }
}

function keyup() {
    lastKeyDown = -1;

    // ctrl
    if(d3.event.keyCode === 16) {
        circle
            .on('mousedown.drag', null)
            .on('touchstart.drag', null);
        svg.classed('ctrl', false);
    }
}

//Travar node ao movimentar
function dragstart(d) {
    d3.select(this).classed("fixed", d.fixed = true);
}

// app starts here
svg.on('mousedown', mousedown)
    .on('mousemove', mousemove)
    .on('mouseup', mouseup);
d3.select(window)
    .on('keydown', keydown)
    .on('keyup', keyup);
restart();

function exibe(){
    var resultado = "",
        nod = "";
    for (propriedade in links){
        resultado += "Linha "+ propriedade + "\n"+
            "Source: " + links[propriedade].source.id + ", Target: " + links[propriedade].target.id + "\n" +
            "Texto Link: " + links[propriedade].need + "\n";
    }
    document.getElementById('arrow').value = resultado;

    for (prop in nodes){
        nod += "Node " + nodes[prop].id + ", Inicial: " + nodes[prop].start + ", Final: " + nodes[prop].end + "\n";
    }
    document.getElementById('node').value = nod;
}

function convertejson(){
	var jnode = JSON.stringify(nodes);
	var jlink = JSON.stringify(links);
	
	document.write('Links: ' + jlink +' <br> node: ' + jnode);
	
}

function initnode(){
	if (selected_node){
		removeInit();
		selected_node.start = !selected_node.start;
		restart();		
	}
}

function endnode(){
	if(selected_node) {
                // toggle node reflexivity
                selected_node.end = !selected_node.end;
            } else if(selected_link) {
                // set link direction to right only
                selected_link.left = false;
                selected_link.right = true;
            }
            restart();
}

function deletenode(){
	if(selected_node) {
                nodes.splice(nodes.indexOf(selected_node), 1);
                spliceLinksForNode(selected_node);
            } else if(selected_link) {
                links.splice(links.indexOf(selected_link), 1);
            }
            selected_link = null;
            selected_node = null;
            restart();
}

//Context Menu
 $(function() {
        $.contextMenu({
            selector: '.node', 
            callback: function(key, options) {
                var m = "clicked: " + key;
                window.console && console.log(m) || alert(m); 
            },
            items: {
				"inicial":{
					name: "Inicial", 
					callback:function(){
						initnode();
					}
				},
				"final":{
					name: "Final",
					callback:function(){
						endnode();
					}
				},
				"move":{ name :"Mover",
						callback:function(){
							 circle.call(drag);
						}
				},
                
                "delete": {name: "Delete", icon: "delete",
							callback:function(){
								deletenode();
							}
					},
                "sep1": "---------",
                "quit": {name: "Quit", icon: function(){
                    return 'context-menu-icon context-menu-icon-quit';
                }}
				
			}
        });
		
		$.contextMenu({
            selector: '.local', 
			 
            items: {
                "new": {name: "Novo node", 
				callback: function(){
					{
						svg.classed('active', true);
						node = {id: ++lastNodeId, end: false, start : false};
						node.x = width/2;
						node.y = height/2;
						node.fixed = true;
						nodes.push(node);
					restart();	
					}
				}
				}
			}
		});

        $('.context-menu-one').on('click', function(e){
            console.log('clicked', this);
        })    
    });