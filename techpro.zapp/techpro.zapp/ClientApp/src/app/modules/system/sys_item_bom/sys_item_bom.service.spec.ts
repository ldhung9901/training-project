import { inject, TestBed } from "@angular/core/testing";
import { BOMService } from "./sys_item_bom.service";

describe('BOMService', ()=>{
    beforeEach(()=> {
        TestBed.configureTestingModule({
            providers: [BOMService]
        });
    });
    it('should be created', inject([BOMService],(service: BOMService)=>{
        expect(service).toBeTruthy();
    }));
});
