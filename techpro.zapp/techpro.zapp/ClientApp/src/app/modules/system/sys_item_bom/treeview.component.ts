import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ActivatedRoute } from '@angular/router';
import { MatTreeFlattener, MatTreeFlatDataSource } from '@angular/material/tree';
import { FlatTreeControl } from '@angular/cdk/tree';
import { SelectionModel } from '@angular/cdk/collections';
import { BehaviorSubject } from 'rxjs';

export class TreeItemNode {
    children: TreeItemNode[];
    db: any;
    index_string: string;
    createby_name: string;
    sys_item_name: string;
    sys_item_unit_name: string;
    sys_item_bom_name: string;
    sys_item_bom_unit_name: string;
    quota_cal: number;
}

export class TreeItemFlatNode {
    db: any;
    level: number;
    expandable: boolean;
    index_string: string;
    createby_name: string;
    sys_item_name: string;
    sys_item_unit_name: string;
    sys_item_bom_name: string;
    sys_item_bom_unit_name: string;
    quota_cal: number;
}

@Component({
    selector: 'sys_item_bom_treeview',
    templateUrl: 'treeview.component.html',
})
export class sys_item_bom_treeviewComponent extends BasePopUpAddComponent {



    item_bom_chose: any;
    public tree_data: any;
    public date_generate: any;
    constructor(public dialogRef: MatDialogRef<sys_item_bom_treeviewComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        fuseConfirmationService: FuseConfirmationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, fuseConfirmationService, route,baseUrl,http,'sys_item_bom', dialogRef);
        this.record = data;

        var that = this;
        that.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel,
            that.isExpandable, this.getChildren);
        that.treeControl = new FlatTreeControl<TreeItemFlatNode>(this.getLevel, this.isExpandable);
        that.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
        this.http
            .post('/sys_item_bom.ctr/get_bom_tree/', {
                id_item_parent: this.record.id
            }
        ).subscribe(resp => {


                that.tree_data = resp;
            that.date_generate = this.buildFileTree(this.tree_data, '1');
            debugger;
                that.dataSource.data = this.date_generate;
            });






    }
    dataChange = new BehaviorSubject<TreeItemNode[]>([]);
    /** Map from flat node to nested node. This helps us finding the nested node to be modified */
    flatNodeMap = new Map<TreeItemFlatNode, TreeItemNode>();

    /** Map from nested node to flattened node. This helps us to keep the same object for selection */
    nestedNodeMap = new Map<TreeItemNode, TreeItemFlatNode>();

    /** A selected parent node to be inserted */
    selectedParent: TreeItemFlatNode | null = null;

    /** The new item's name */
    newItemName = '';

    treeControl: FlatTreeControl<TreeItemFlatNode>;

    treeFlattener: MatTreeFlattener<TreeItemNode, TreeItemFlatNode>;

    dataSource: MatTreeFlatDataSource<TreeItemNode, TreeItemFlatNode>;

    /** The selection for checklist */
    checklistSelection = new SelectionModel<TreeItemFlatNode>(false /* multiple */);


    getLevel = (node: TreeItemFlatNode) => node.level;

    isExpandable = (node: TreeItemFlatNode) => node.expandable;

    getChildren = (node: TreeItemNode): TreeItemNode[] => node.children;

    hasChild = (_: number, _nodeData: TreeItemFlatNode) => _nodeData.expandable;

    /**
     * Transformer to convert nested node to flat node. Record the nodes in maps for later use.
     */
    transformer = (node: TreeItemNode, level: number) => {
        const existingNode = this.nestedNodeMap.get(node);
        const flatNode = existingNode && existingNode.sys_item_bom_name === node.sys_item_bom_name
            ? existingNode
            : new TreeItemFlatNode();
        flatNode.db = node.db;
        flatNode.level = level;
        flatNode.index_string = node.index_string;
        flatNode.createby_name = node.createby_name;
        flatNode.sys_item_name = node.sys_item_name;
        flatNode.sys_item_unit_name = node.sys_item_unit_name;
        flatNode.sys_item_bom_name = node.sys_item_bom_name;
        flatNode.sys_item_bom_unit_name = node.sys_item_bom_unit_name;
        flatNode.quota_cal = node.quota_cal;

        flatNode.expandable = node.children && node.children.length > 0;
        this.flatNodeMap.set(flatNode, node);
        this.nestedNodeMap.set(node, flatNode);
        return flatNode;
    }

    /** Whether all the descendants of the node are selected */
    descendantsAllSelected(node: TreeItemFlatNode): boolean {
        const descendants = this.treeControl.getDescendants(node);
        return descendants.every(child => this.checklistSelection.isSelected(child));
    }

    /** Whether part of the descendants are selected */
    descendantsPartiallySelected(node: TreeItemFlatNode): boolean {
        const descendants = this.treeControl.getDescendants(node);
        const result = descendants.some(child => this.checklistSelection.isSelected(child));
        return result && !this.descendantsAllSelected(node);
    }

    /** Toggle the to-do item selection. Select/deselect all the descendants node */
    todoItemSelectionToggle(node: TreeItemFlatNode): void {
        this.checklistSelection.toggle(node);
        const descendants = this.treeControl.getDescendants(node);
        this.checklistSelection.isSelected(node)
            ? this.checklistSelection.select(...descendants)
            : this.checklistSelection.deselect(...descendants);
    }

    filterChanged(filterText: string) {
        this.filter(filterText);
        if (filterText) {
            this.treeControl.expandAll();
        } else {
            this.treeControl.collapseAll();
        }
    }

    buildFileTree(obj: any[], level: string): TreeItemNode[] {

        return obj.filter(o =>
            (<string>o.index_string).startsWith(level + '.')
            && (o.index_string.match(/\./g) || []).length === (level.match(/\./g) || []).length + 1
        )
            .map(o => {

                const node = new TreeItemNode();
                node.index_string = o.index_string;
                node.db = o.db;
                node.createby_name = o.createby_name;
                node.sys_item_name = o.sys_item_name;
                node.sys_item_unit_name = o.sys_item_unit_name;
                node.sys_item_bom_name = o.sys_item_bom_name;
                node.sys_item_bom_unit_name = o.sys_item_bom_unit_name ;
                node.quota_cal = o.quota_cal;
                const children = obj.filter(so => (<string>so.index_string).startsWith(level + '.'));
                if (children && children.length > 0) {
                    node.children = this.buildFileTree(children, o.index_string);
                }
                return node;
            });
    }
    public filter(filterText: string) {
        let filteredTreeData;
        if (filterText) {
            debugger;
            filteredTreeData = this.tree_data.filter(d => d.sys_item_bom_name.toLocaleLowerCase().indexOf(filterText.toLocaleLowerCase()) > -1);
            Object.assign([], filteredTreeData).forEach(ftd => {
                let str = (<string>ftd.index_string);
                while (str.lastIndexOf('.') > -1) {
                    const index = str.lastIndexOf('.');
                    str = str.substring(0, index);
                    if (filteredTreeData.findIndex(t => t.index_string === str) === -1) {
                        const obj = this.tree_data.find(d => d.index_string === str);
                        if (obj) {
                            filteredTreeData.push(obj);
                        }
                    }
                }
            });
        } else {
            filteredTreeData = this.tree_data;
        }

        // Build the tree nodes from Json object. The result is a list of `TodoItemNode` with nested
        // file node as children.
        this.date_generate = this.buildFileTree(filteredTreeData, '0');
        // Notify the change.
        this.dataChange.next(this.date_generate);
    }

}

