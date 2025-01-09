import { BaseFilter } from "../base.model";

export class OrderModel extends BaseFilter {
    id?: string;
    stt?: number;
    vehicleCode?: string;
    name?: string;
    headerId?: any;
    count?: string;
    order?: string;
    hasMore?: boolean = true;
}