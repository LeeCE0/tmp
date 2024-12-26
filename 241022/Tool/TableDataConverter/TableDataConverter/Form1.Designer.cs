
namespace TableDataConverter
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        private System.Windows.Forms.TextBox textBox_console;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_console = new System.Windows.Forms.TextBox();
            this.convertBtn = new System.Windows.Forms.Button();

            // button_convert
            this.convertBtn.Location = new System.Drawing.Point(185, 300);
            this.convertBtn.Margin = new System.Windows.Forms.Padding(4);
            this.convertBtn.Name = "button_convert";
            this.convertBtn.Size = new System.Drawing.Size(588, 51);
            this.convertBtn.TabIndex = 20;
            this.convertBtn.Text = "Convent";
            this.convertBtn.UseVisualStyleBackColor = true;
            this.convertBtn.Click += new System.EventHandler(this.ClickConvert);

            // textBox_console
            this.textBox_console.Location = new System.Drawing.Point(13, 406);
            this.textBox_console.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_console.Multiline = true;
            this.textBox_console.Name = "textBox_console";
            this.textBox_console.ReadOnly = true;
            this.textBox_console.Size = new System.Drawing.Size(900, 363);
            this.textBox_console.TabIndex = 16;
            this.textBox_console.WordWrap = false;

            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 821);
            this.Text = "Form1";

            this.Controls.Add(this.convertBtn);
            this.Controls.Add(this.textBox_console);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        //private System.Windows.Forms.Button convertBtn;
    }
}

