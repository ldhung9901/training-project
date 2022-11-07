/* tslint:disable:max-line-length */
import { FuseNavigationItem } from '@fuse/components/navigation';
const itemNAV: FuseNavigationItem[] = [{
    id: 'inventory',
    module:'inventory',
    title: 'NAV.inventory',
    translate: 'NAV.inventory',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }
    // ],
},
{
    id: 'buyorder',
    module:'buyorder',
    title: 'NAV.buyorder',
    translate: 'NAV.buyorder',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }

    // ],
},
{
    id: 'business',
    module:'business',
    title: 'NAV.business',
    translate: 'NAV.business',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }

    // ],
},
{
    id: 'production',
    module:'production',
    title: 'NAV.production',
    translate: 'NAV.production',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }

    // ],
},
{
    id: 'system',
    module:'system',
    title: 'NAV.system',
    translate: 'NAV.system',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }

    // ],
},
{
    id: 'maintenance',
    module:'maintenance',
    title: 'NAV.maintenance',
    translate: 'NAV.maintenance',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }

    // ],

},
{
    id: 'quality',
    module:'quality',
    title: 'NAV.quality',
    translate: 'NAV.quality',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }],

},
{
    id: 'tool',
    module:'tool',
    title: 'NAV.tool',
    translate: 'NAV.tool',
    type: 'group',
    // children: [{
    //     icon: 'receipt',
    //     id: 'report',
    //     title: 'report',
    //     translate: 'NAV.report',
    //     type: 'collapsable',
    //     children: [],
    // }],

}];
export const defaultNavigation: FuseNavigationItem[] = itemNAV;
export const compactNavigation: FuseNavigationItem[] = itemNAV;
export const futuristicNavigation: FuseNavigationItem[] = itemNAV;
export const horizontalNavigation: FuseNavigationItem[] = itemNAV;
