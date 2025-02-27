import { Component, ChangeDetectorRef } from '@angular/core'
import { NzIconModule } from 'ng-zorro-antd/icon'
import { NzLayoutModule } from 'ng-zorro-antd/layout'
import { NzMenuModule } from 'ng-zorro-antd/menu'
import { NzDropDownModule } from 'ng-zorro-antd/dropdown'
import { NzAvatarModule } from 'ng-zorro-antd/avatar'
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb'
import { NzSpinModule } from 'ng-zorro-antd/spin'
import { RouterOutlet } from '@angular/router'
import { CommonModule } from '@angular/common'
import { GlobalService } from '../../services/global.service'
import { AuthService } from '../../services/auth.service'
import { SidebarMenuService } from '../../services/sidebar-menu.service'
import { Router } from '@angular/router'
import { RouterModule } from '@angular/router'
import { DropdownService } from '../../services/dropdown/dropdown.service'
import { ShareModule } from '../../shared/share-module'
import { OrganizeService } from '../../services/system-manager/organize.service'
import { WarehouseService } from '../../services/master-data/warehouse.service'
import { COMPANY_CODE_TAP_DOAN } from '../../shared/constants'

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    NzLayoutModule,
    NzMenuModule,
    NzIconModule,
    NzSpinModule,
    RouterOutlet,
    CommonModule,
    NzDropDownModule,
    NzAvatarModule,
    NzBreadCrumbModule,
    RouterModule,
    ShareModule
  ],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss'],
})
export class MainLayoutComponent {
  isCollapsed = false
  userName: string = 'User'
  dataSidebarMenu: any[] = []
  openIndex: number | null = null
  currentUrl: string = ''
  breadcrumbs: any = this.globalService.breadcrumb || []
  loading: boolean = false
  screenWidth: number = window.innerWidth
  listAccount: any[] = []

  lstCompanyCode : any[] = []
  selectedCompanyCode? : string =localStorage.getItem('companyCode')?.toString()

  lstWarehouse : any[] = []
  selectedWarehouse? : string =localStorage.getItem('warehouseCode')?.toString()

  _companyCode : any = localStorage.getItem('UserInfo')
  companyCode = JSON.parse(this._companyCode)?.organizeCode

  isDisableSelect : boolean = this.companyCode.toString() == COMPANY_CODE_TAP_DOAN ? false : true

  constructor(
    private globalService: GlobalService,
    private dropdownService: DropdownService,
    private authService: AuthService,
    private router: Router,
    private sidebarMenuService: SidebarMenuService,
    private cdr: ChangeDetectorRef,
    private _sOrg: OrganizeService,
    private _sWah : WarehouseService
  ) {
    this.userName = this.globalService.getUserInfo().userName
    this.globalService.rightSubject.subscribe((item) => {
      this.getSidebarMenu()
    })
    this.globalService.breadcrumbSubject.subscribe((value) => {
      this.breadcrumbs = value
    })
    this.globalService.getLoading().subscribe((value) => {
      this.loading = value
    })
    this.router.events.subscribe(() => {
      this.currentUrl = this.router.url?.split('?')[0] || ''
      this.dataSidebarMenu = this.transformMenuList(this.dataSidebarMenu)
      this.cdr.detectChanges()
    })
  }
  ngOnDestroy() {
    window.removeEventListener('resize', this.onResize)
  }

  onResize = () => {
    this.screenWidth = window.innerWidth
    this.cdr.detectChanges()
  }

  transformMenuList(data: any[], level = 1): any[] {
    return data.map((menu) => this.transformMenu(menu, level))
  }

  transformMenu(data: any, level = 0): any {
    const hasMatchingChild = (menu: any, url: string): boolean => {
      if (menu.url && `/${menu.url}` === url) {
        return true
      }
      if (menu.children) {
        return menu.children.some((child: any) => hasMatchingChild(child, url))
      }
      return false
    }

    return {
      level: level,
      title: data.name || data.title || '',
      icon: data.icon || '',
      open: hasMatchingChild(data, this.currentUrl),
      url: data.url,
      selected: `/${data.url}` === this.currentUrl,
      disabled: false,
      children: data.children
        ? this.transformMenuList(data.children, level + 1)
        : undefined,
    }
  }

  ngAfterViewInit(): void {
    this.isCollapsed = localStorage.getItem('openSidebar')
      ? localStorage.getItem('openSidebar') === 'true'
      : false
  }

  ngOnInit(): void {
    window.addEventListener('resize', this.onResize)
    this.getSidebarMenu()
    this.currentUrl = this.router.url
    this.getAllAccount();
    this.getOrganize();
    this.companyCodeDefault(this.selectedCompanyCode)
  }

  logout(): void {
    this.authService.logout()
    this.router.navigate(['/login'])
  }
  changePass(): void {
    this.router.navigate(['/system-manager/profile'])
  }

  getSidebarMenu(): void {
    // this.sidebarMenuService
    //   .getMenuOfUser({
    //     userName: this.userName,
    //   })
    //   .subscribe((res) => {
    //     this.dataSidebarMenu = this.transformMenuList(res?.children || [])
    //   })
    this.sidebarMenuService
      .getMenu().subscribe((res : any) => {
        this.dataSidebarMenu = this.transformMenuList(res?.children || [])
      })
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed
    localStorage.setItem('openSidebar', this.isCollapsed ? 'true' : 'false')
  }

  toggleOpen(index: number): void {
    this.openIndex = this.openIndex === index ? null : index
  }

  navigateTo(url: string): void {
    if (url && !this.loading) {
      this.router.navigate([url])
    }
  }
  getAllAccount() {
    this.dropdownService.GetAllAccount().subscribe({
      next: (data) => {
        this.listAccount = data
      },
      error: (response) => {
        console.log(response)
      },
    })
  }
  getAccountName(userName: string): string {
    const listAccount = this.listAccount.find(item => item.userName === userName);
    return listAccount ? listAccount.fullName : 'N/A';
  }

  getOrganize() {
    this._sOrg.getOrg().subscribe({
      next: (res : any) => {
       this.lstCompanyCode = res
      }
    })
  }

  companyCodeDefault(value: any): void {
    localStorage.setItem('companyCode', value)
    this._sWah.getByOrg(value).subscribe({
      next: (res) => {
        this.lstWarehouse = res  
        if(this.lstWarehouse.filter(x => x.code == this.selectedWarehouse).length == 0){
          this.selectedWarehouse = res[0]?.code
          localStorage.setItem('warehouseCode', res[0]?.code)
        }
      }
    })
  }

  companyCodeChange(value: any): void {
    localStorage.setItem('companyCode', value)
    this._sWah.getByOrg(value).subscribe({
      next: (res) => {
        this.lstWarehouse = res  
        if(this.lstWarehouse.filter(x => x.code == this.selectedWarehouse).length == 0){
          this.selectedWarehouse = res[0]?.code
          localStorage.setItem('warehouseCode', res[0]?.code)
        }
        window.location.reload();
      }
    })
  }
  warehouseChange(value: any): void {

    localStorage.setItem('warehouseCode', value)
    window.location.reload();
  }
}
